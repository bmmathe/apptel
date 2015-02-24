using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

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
            string baseAddress = "http://localhost:9086/";
            WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine("Test Api running at: {0}", baseAddress);
            Console.WriteLine("Press enter to run pulse test");
            while (Console.ReadLine() != "X")
            {
                RunAsync().Wait();
                Console.WriteLine("Pulse sent to server.");
            }            
        }

        private static async Task RunAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["AppTelServiceUrl"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    await client.PostAsJsonAsync("api/Pulse", new Pulse {ApplicationName = "Test Application", EndPoint = "TestEndpoint", ElapsedTime = 2});
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred when sending a pulse to the AppTel Service API.");
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
