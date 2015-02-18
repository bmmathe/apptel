using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AppTel.Domain.Data
{
    [BsonIgnoreExtraElements]
    public class AppMetrics
    {
        public AppMetrics()
        {
            Pings = new List<AppPing>();
            Pulses = new List<Pulse>();
            Exceptions = new List<AppError>();
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public int PingsPerHour { get; set; }
        public int PulsesPerHour { get; set; }
        public List<AppPing> Pings { get; set; }
        public List<Pulse> Pulses { get; set; }
        public List<AppError> Exceptions { get; set; } 
    }
}