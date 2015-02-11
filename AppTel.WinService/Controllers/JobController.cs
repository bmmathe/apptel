using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using AppTel.WinService.Jobs;
using AppTel.WinService.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace AppTel.WinService.Controllers
{
    [RoutePrefix("api/Job")]
    public class JobController : ApiController
    {
        [HttpGet]
        public JobModel[] Get()
        {
            var jobs = new List<JobModel>();
            var scheduler = new StdSchedulerFactory().GetScheduler();
            IList<string> jobGroups = scheduler.GetJobGroupNames();

            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher);
                jobs.AddRange(jobKeys.Select(GetJobDetails));
            }
            return jobs.ToArray();
        }

        [HttpGet]
        [Route("{jobName}")]
        public JobModel Get(string jobName)
        {
            return GetJobDetails(new JobKey(jobName, "group1"));
        }

        private JobModel GetJobDetails(JobKey jobKey)
        {
            var job = new JobModel();
            var scheduler = new StdSchedulerFactory().GetScheduler();
            var detail = scheduler.GetJobDetail(jobKey);
            job.ApplicationName = detail.JobDataMap.GetString("ApplicationName");
            job.Endpoint = detail.JobDataMap.GetString("Endpoint");
            job.JobName = detail.Key.Name;
            var trigger = scheduler.GetTriggersOfJob(jobKey).First();
            job.TriggerName = trigger.Key.Name;

            var simpleTrigger = trigger as ISimpleTrigger;
            if (simpleTrigger != null)
            {
                job.RepeatIntervalInSeconds = simpleTrigger.RepeatInterval.Seconds;
            }
            
            DateTimeOffset? nextFireTime = trigger.GetNextFireTimeUtc();
            if (nextFireTime.HasValue)
            {
                job.NextFireTime = nextFireTime.Value.LocalDateTime.ToString();
            }

            DateTimeOffset? previousFireTime = trigger.GetPreviousFireTimeUtc();
            if (previousFireTime.HasValue)
            {
                job.PreviousFireTime = previousFireTime.Value.LocalDateTime.ToString();
            }

            var triggerState = scheduler.GetTriggerState(trigger.Key);            
            job.TriggerState = triggerState.ToString();

            return job;
        }

        public void Post([FromBody] JobModel model)
        {
            var job = JobBuilder.Create<PingJob>()
                .WithIdentity(string.Format("ping_{0}", model.ApplicationName), "group1") // name "myJob", group "group1"
                .UsingJobData("ApplicationName", model.ApplicationName)
                .UsingJobData("Endpoint", model.Endpoint)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(string.Format("trigger_{0}", model.ApplicationName), "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(model.RepeatIntervalInSeconds)
                    .RepeatForever())
                .Build();

            var schedule = new StdSchedulerFactory().GetScheduler();
            schedule.ScheduleJob(job, trigger);
        }

        public void Put([FromBody] JobModel model)
        {
            var scheduler = new StdSchedulerFactory().GetScheduler();
            var job  = scheduler.GetJobDetail(new JobKey(model.JobName));
            var trigger = scheduler.GetTrigger(new TriggerKey(model.TriggerName));
            var builder = trigger.GetTriggerBuilder();
            var newTrigger = builder.WithSimpleSchedule(x => x.WithIntervalInSeconds(model.RepeatIntervalInSeconds).RepeatForever()).Build();
            scheduler.RescheduleJob(trigger.Key, newTrigger);            
        }
        
        [Route("{jobName}")]
        public IHttpActionResult Delete(string jobName)
        {
            try
            {
                var scheduler = new StdSchedulerFactory().GetScheduler();
                scheduler.DeleteJob(new JobKey(jobName, "group1"));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok();
        }

        [HttpPost]
        [Route("api/Job/pause/{jobName}")]
        public IHttpActionResult PauseJob(string jobName)
        {
            var scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.PauseJob(new JobKey(jobName, "group1"));
            return Ok();
        }

        [HttpPost]
        [Route("api/Job/resume/{jobName}")]
        public IHttpActionResult ResumeJob(string jobName)
        {
            var scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.ResumeJob(new JobKey(jobName, "group1"));
            return Ok();
        }
    }
}
