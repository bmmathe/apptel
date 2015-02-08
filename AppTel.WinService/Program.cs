using System;
using Microsoft.Owin.Hosting;
using Quartz.Impl;

namespace AppTel.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9085/";
            WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine("Service running. Starting Quartz scheduler.");
            var schedule = new StdSchedulerFactory().GetScheduler();
            schedule.Start();            

            Console.ReadLine(); 
        }
    }    
}
