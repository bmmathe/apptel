using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using AppTel.Web.Models;

namespace AppTel.Web.Controllers
{
    public class PingScheduleController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var viewModel = await GetPingScheduleViewModel();            
            return View(viewModel);
        }

        private async Task<PingScheduleViewModel> GetPingScheduleViewModel()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync("api/Job");
                var jobs = await response.Content.ReadAsAsync<List<JobModel>>();
                var viewModel = new PingScheduleViewModel {Jobs = jobs, AppTelServiceBaseURL = ConfigurationManager.AppSettings["AppTelServiceBaseURL"]};
                return viewModel;
            }
        }

        public ActionResult Create()
        {
            return View(new JobModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(JobModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.PostAsJsonAsync("api/Job", model).ContinueWith(posttask => posttask.Result.EnsureSuccessStatusCode());                
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.DeleteAsync(string.Format("api/Job/{0}", id)).ContinueWith(posttask => posttask.Result.EnsureSuccessStatusCode());
            }
            var viewModel = await GetPingScheduleViewModel();
            return PartialView("_ScheduleTable", viewModel);
        }

        public async Task<ActionResult> Pause(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.PostAsync(string.Format("api/Job/Pause/{0}", id), null).ContinueWith(posttask => posttask.Result.EnsureSuccessStatusCode());
            }
            var viewModel = await GetPingScheduleViewModel();
            return PartialView("_ScheduleTable", viewModel);
        }

        public async Task<ActionResult> Resume(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.PostAsync(string.Format("api/Job/Resume/{0}", id), null).ContinueWith(posttask => posttask.Result.EnsureSuccessStatusCode());
            }
            var viewModel = await GetPingScheduleViewModel();
            return PartialView("_ScheduleTable", viewModel);
        }
    }    
}