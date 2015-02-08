using System.Configuration;
using System.Linq;
using AppTel.Domain.Data;
using AppTel.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace AppTel.Domain.Tests
{

    [TestClass]
    public class MongoTests
    {        
        [TestMethod]
        public void Ping_InsertsNewApplicationDocument()
        {            
            var pingService = new PingService();
            pingService.Ping("Leap", "http://www.google.com");
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabaseNames();
            Assert.IsTrue(database.Contains(Constants.DatabaseName));
        }

        [TestMethod]
        [ExpectedException(typeof(MongoDuplicateKeyException))]
        public void InitializeAppTelDb_UniqueIndexCreated()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            database.Drop();

            var databases = server.GetDatabaseNames();
            Assert.IsFalse(databases.Contains(Constants.DatabaseName));

            InitializeAppTelDb.Initialize();
            
            var collection = database.GetCollection(Constants.CollectionName);
            collection.Insert(new AppMetrics {Name = "App1"});
            collection.Insert(new AppMetrics {Name = "App1"});
            
        }

        [TestMethod]
        public void GetUniqueApplications()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>(Constants.CollectionName);
            var apps = collection.FindAll().SetFields("Name");
            foreach (var app in apps)
            {
                var s = app.Name;
            }            
        }
    }
}
