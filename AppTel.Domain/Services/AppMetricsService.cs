using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AppTel.Domain.Data;
using AppTel.Domain.Models;
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

        public IEnumerable<AppMetricsQuickCheckInfo> GetAllAppsQuickCheck()
        {
            var appMetricsCheckList = new List<AppMetricsQuickCheckInfo>();
            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>("applications");
            var apps = collection.FindAll().SetFields(Fields<AppMetrics>.Include(a => a.Name).Slice(s => s.Pings, 10)).ToList();
            return appMetricsCheckList;
        }
    }
}