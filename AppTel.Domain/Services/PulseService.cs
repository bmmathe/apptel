using System;
using System.Collections.Generic;
using System.Configuration;
using AppTel.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Services
{
    public class PulseService : IPulseService
    {                
        public List<Pulse> GetPulses(string applicationName)
        {
            return new List<Pulse>();
        }

        public void Pulse(string applicationName, string endpoint, int? elapsedTime)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>("applications");
            var query = Query<AppMetrics>.EQ(e => e.Name, applicationName);
            var update = Update<AppMetrics>.AddToSet(e => e.Pulses, new Pulse() { CreatedDate = DateTime.Now, Endpoint = endpoint, ElapsedTime = elapsedTime }).Inc(e => e.PulsesPerHour, 1);
            collection.Update(query, update, UpdateFlags.Upsert);
        }
    }
}