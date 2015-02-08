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
                foreach (var jobKey in jobKeys)
                {
                    var job = new JobModel();                    
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
                    jobs.Add(job);
                }
            }
            return jobs.ToArray();
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
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                .Build();

            var schedule = new StdSchedulerFactory().GetScheduler();
            schedule.ScheduleJob(job, trigger);
        }
    }
}
