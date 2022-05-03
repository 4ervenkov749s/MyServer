using Confluent.Kafka;
using MessagePack;
using System;

namespace RabbitMqService.BL.Services
{
    internal class MsgPackDeserializer<T> : IDeserializer<T>
    {
        
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
