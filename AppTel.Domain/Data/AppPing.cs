using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AppTel.Domain.Data
{
    [BsonIgnoreExtraElements]
    public class AppPing
    {
        public DateTime CreatedDate { get; set; }
        public string Endpoint { get; set; }
        public bool IsSuccess { get; set; }
        public string Error { get; set; }        
    }
}
