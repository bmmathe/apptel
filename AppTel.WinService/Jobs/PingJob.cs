using System;
using AppTel.Domain.Services;
using Quartz;

namespace AppTel.WinService.Jobs
{
    public class PingJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var applicationName = dataMap.GetString("ApplicationName");
            var endpoint = dataMap.GetString("Endpoint");
            var detectText = dataMap.GetString("DetectText");
            var pingService = new PingService();
            pingService.Ping(applicationName, endpoint, detectText);
            Console.WriteLine("Ran ping test for {0} at {1}", applicationName, endpoint);
        }
    }
}
