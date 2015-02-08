using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppTel.Web.Models;

namespace AppTel.Web.Controllers
{
    public class PingScheduleController : Controller
    {
        public async  Task<ActionResult> Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/Job");
                var jobs = await response.Content.ReadAsAsync<List<JobModel>>();
                return View(jobs);
            }
        }
    }    
}