using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace AppTel.Domain.Data
{
    [BsonIgnoreExtraElements]
    public class AppError
    {
        public AppError()
        {   
            Data = new Dictionary<string, string>();
        }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Dictionary<string, string> Data { get; set; }

        public AppError InnerException { get; set; }
    }
}
