using System.Configuration;
using AppTel.Domain.Data;
using AppTel.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Tests
{
    [TestClass]
    public class PingTests
    {
        [TestMethod]
        public void Ping_InsertsNewApplicationDocument()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>(Constants.CollectionName);
            var query = Query<AppMetrics>.EQ(e => e.Name, "TestApp1");
            var initialQuery = collection.FindOne(query);
            int pingCount = 0;
            if(initialQuery !=null)
                pingCount = initialQuery.Pings.Count;

            var pingService = new PingService();
            pingService.Ping("TestApp1", "https://www.google.com");

            
            var afterQuery = collection.FindOne(query);

            Assert.AreEqual("TestApp1", afterQuery.Name);
            Assert.AreEqual(pingCount+1, afterQuery.Pings.Count);

            //collection.Remove(query);
        }
    }
}
