using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AppTel.WinService.Jobs;
using AppTel.WinService.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace AppTel.WinService.Controllers
{
    public class JobController : ApiController
    {
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

        public JobModel Get(string jobName)
        {
            return GetJobDetails(new JobKey(jobName));
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
            if(model.IsPaused)
                scheduler.PauseJob(job.Key);
            else
            {
                scheduler.ResumeJob(job.Key);                
            }
        }
    }
}
