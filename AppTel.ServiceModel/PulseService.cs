using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using AppTel.ServiceModel;

namespace AppTel.Client
{
    public class PulseService
    {
        public void LogPulse(AppPulse pulse)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.PostAsJsonAsync("api/Pulse", pulse);
            }
        }
    }
}
