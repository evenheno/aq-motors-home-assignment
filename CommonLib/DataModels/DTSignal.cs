using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonLib.DataModels
{
    [BsonDiscriminator("DTSignal")]
    public record DTSignal
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? id { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("Type")]
        [JsonPropertyName("signalType")]
        public virtual EnSignalType SignalType { get; }
    }
}

