using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonLib.DataModels
{
    [BsonDiscriminator("DTSine")]
    public record DTSine : DTSignal
    {
        [BsonElement("Frequency")]
        [JsonPropertyName("frequency")]
        public int Frequency { get; set; }

        [BsonElement("Amplitude")]
        [JsonPropertyName("amplitude")]
        public int Amplitude { get; set; }
        public new EnSignalType SignalType { get; } = EnSignalType.Sine;
    }

}

