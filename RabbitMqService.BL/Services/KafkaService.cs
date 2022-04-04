using Confluent.Kafka;
using RabbitMqService.BL.Interfaces;
using RabbitMqService.Models;
using System.Threading.Tasks;

namespace RabbitMqService.BL.Services
{
    public class KafkaService : IKafkaService
    {

        private static IProducer<int, Person> _producer;
        
        public KafkaService()
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "localhost:9092",
            }; 
            _producer = new ProducerBuilder<int, Person>(config)
                .SetValueSerializer(new MsgPackSerializer<Person>())
                .Build();
        }
        public async Task SendPersonAsync(Person p)
        {
            var result = await _producer.ProduceAsync("test", new Message<int, Person>()
            {
                Key = p.Id,
                Value = p
            });
        }
    }
}
