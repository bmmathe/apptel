using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AppTel.WinService.Test
{
    public class Pulse
    {
        public string ApplicationName { get; set; }
        public string EndPoint { get; set; }
        public int? ElapsedTime { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to run pulse test");
            while (Console.ReadLine() != "X")
            {
                RunAsync().Wait();                
            }            
        }

        private static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await client.PostAsJsonAsync("api/Pulse", new Pulse() { ApplicationName = "Test Application", EndPoint = "TestEndpoint" });
            }
        }
    }
}
