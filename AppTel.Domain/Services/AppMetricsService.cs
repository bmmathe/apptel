using System.Configuration;
using AppTel.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Services
{
    public class AppMetricsService : IAppMetricsService
    {
        public AppMetrics GetAppMetrics(string applicationName)
        {
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>("applications");
            var query = Query<AppMetrics>.EQ(e => e.Name, applicationName);
            return collection.FindOne(query);
        }
    }
}