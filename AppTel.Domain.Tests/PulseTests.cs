using System.Configuration;
using AppTel.Domain.Data;
using AppTel.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Tests
{
    [TestClass]
    public class PulseTests
    {
        [TestMethod]
        public void Pulse_InsertsNewApplicationDocument()
        {            
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>(Constants.CollectionName);
            var query = Query<AppMetrics>.EQ(e => e.Name, "TestApp1");
            var initialQuery = collection.FindOne(query);
            int pulseCount = 0;
            if (initialQuery != null)
                pulseCount = initialQuery.Pulses.Count;

            var pulseService = new PulseService();
            pulseService.Pulse("TestApp1", "https://www.google.com", 10);

            var afterQuery = collection.FindOne(query);

            Assert.AreEqual("TestApp1", initialQuery.Name);
            Assert.AreEqual(pulseCount + 1, afterQuery.Pulses.Count);

            collection.Remove(query);
        }
    }
}
