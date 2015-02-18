using System;
using System.Configuration;
using AppTel.Domain.Data;
using AppTel.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Tests
{
    [TestClass]
    public class ErrorServiceTests
    {
        [TestMethod]
        public void LogError_LogsExceptionToApplication()
        {            
            var exception = new Exception("This is a test exception.");
            exception.Data["dataitem1"] = "datavalue1";
            exception.Data["dataitem2"] = "datavalue2";
            var errorService = new ErrorService();
            errorService.LogError("TestApp1", exception);
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>(Constants.CollectionName);
            var query = Query<AppMetrics>.EQ(e => e.Name, "TestApp1");            
            var entity = collection.FindOne(query);

            Assert.AreEqual("TestApp1", entity.Name);

            var exceptionFromDb = entity.Exceptions[0];
            Assert.AreEqual("This is a test exception.", exceptionFromDb.Message);
            Assert.AreEqual("datavalue1", exceptionFromDb.Data["dataitem1"]);
            Assert.AreEqual("datavalue2", exceptionFromDb.Data["dataitem2"]);
            //collection.Remove(query);
        }
    }
}
