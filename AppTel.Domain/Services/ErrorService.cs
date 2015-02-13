using System;
using System.Configuration;
using System.Diagnostics;
using AppTel.Domain.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AppTel.Domain.Services
{
    public class ErrorService : IErrorService
    {
        public void LogError(string applicationName, Exception exception)
        {
            var parentException = GetExceptionLogItem(exception);
            var childException = exception.InnerException;
            var currentExceptionLog = parentException;
            while (childException != null)
            {
                var exceptionLogItem = GetExceptionLogItem(childException);
                currentExceptionLog.InnerException = exceptionLogItem;
                childException = childException.InnerException;
                currentExceptionLog = exceptionLogItem;
            }

            var connectionString = ConfigurationManager.AppSettings["MongoDBURI"];
            var dbclient = new MongoClient(connectionString);
            var server = dbclient.GetServer();
            var database = server.GetDatabase(Constants.DatabaseName);
            var collection = database.GetCollection<AppMetrics>("applications");
            var query = Query<AppMetrics>.EQ(e => e.Name, applicationName);
            var update = Update<AppMetrics>.AddToSet(e => e.Exceptions, parentException);
            collection.Update(query, update, UpdateFlags.Upsert);
        }

        private AppError GetExceptionLogItem(Exception ex)
        {
            var appError = new AppError {Message = ex.Message, ExceptionType = ex.GetType().ToString(), StackTrace = ex.StackTrace};
            foreach (var key in ex.Data.Keys)
            {
                try
                {
                    var value = ex.Data[key];
                    if (!appError.Data.ContainsKey(key.ToString()))
                        appError.Data.Add(key.ToString(), value.ToString());
                }
                catch(Exception ie)
                {
                    Debug.WriteLine(ie.GetType().ToString(), ie.Message, ie.StackTrace);
                    // just in case a value can't be added, skip that value and continue
                }
            }
            return appError;
        }
    }
}