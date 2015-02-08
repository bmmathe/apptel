using System.Configuration;
using AppTel.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Services
{
    public static class InitializeAppTelDb
    {
        public static void Initialize()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            if (!server.DatabaseExists(Constants.DatabaseName))
            {
                var db = server.GetDatabase(Constants.DatabaseName);
                var collection = db.GetCollection<AppMetrics>(Constants.CollectionName);
                collection.CreateIndex(IndexKeys.Ascending("Name"), IndexOptions.SetUnique(true));
            }
        }
    }
}
