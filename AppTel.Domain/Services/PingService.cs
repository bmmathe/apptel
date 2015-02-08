using System;
using System.Configuration;
using System.IO;
using System.Net;
using AppTel.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Services
{
    public class PingService : IPingService 
    {
        public void Ping(string applicationName, string endpoint, string detectText = null)
        {
            string error = string.Empty;
            bool isSuccess = false;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    var data = client.OpenRead(endpoint);
                    using (var reader = new StreamReader(data))
                    {
                        var response = reader.ReadToEnd();
                        data.Close();
                        reader.Close();
                        isSuccess = true;
                        if (!string.IsNullOrEmpty(detectText))
                        {
                            isSuccess = response.Contains(detectText);
                        }
                    }                    
                }
                                
            }
            catch (Exception ex)
            {
                error = string.Format("{0}: {1}", ex.GetType(), ex.Message);
            }
            
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>("applications");            
            var query = Query<AppMetrics>.EQ(e => e.Name, applicationName);
            var update = Update<AppMetrics>.AddToSet(e => e.Pings, new AppPing() { IsSuccess = isSuccess, Endpoint = endpoint, CreatedDate = DateTime.Now,  Error = error}).Inc(e => e.PingsPerHour, 1);
            collection.Update(query, update, UpdateFlags.Upsert);          
        }
    }
}
