using MongoDB.Bson;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace CommonLib.DataModels
{
    [BsonDiscriminator("DTState")]
    public record DTState : DTSignal
    {
        public int State { get; set; }

        public new EnSignalType SignalType { get; } = EnSignalType.State;
    }
}