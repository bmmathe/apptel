using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AppTel.Client
{
    public class ErrorLogger
    {
        public void LogError(AppError error)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceBaseURL"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.PostAsJsonAsync("api/Error", error);
            }
        }
    }
}
