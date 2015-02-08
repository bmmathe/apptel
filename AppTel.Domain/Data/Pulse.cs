using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AppTel.Domain.Data
{
    [BsonIgnoreExtraElements]
    public class Pulse
    {
        public DateTime CreatedDate { get; set; }
        public string Endpoint { get; set; }
        public long? ElapsedTime { get; set; }
    }
}