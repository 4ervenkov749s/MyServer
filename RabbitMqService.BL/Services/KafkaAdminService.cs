using Confluent.Kafka;
using Confluent.Kafka.Admin;
using RabbitMqService.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.BL.Services
{
    public class KafkaAdminService : IKafkaAdminService, IDisposable
    {
        private readonly AdminClientBuilder _adminClientBuilder;
        private readonly IAdminClient _adminClient;

        public KafkaAdminService()
        {
            var config = new AdminClientConfig()
            {
                BootstrapServers = "localhost:9092"
            };

            _adminClientBuilder = new AdminClientBuilder(config);

            _adminClient = _adminClientBuilder.SetErrorHandler((client, error) =>
            {
                Console.WriteLine($"Client: {client.Name} Error: {error.Reason}");
            }).Build();
        }

        public async Task CreateTopicAsync(string topicName)
        {
            await _adminClient.CreateTopicsAsync(new List<TopicSpecification>()
            {
                new TopicSpecification()
                {
                    Name = topicName,
                    NumPartitions = 1,
                    ReplicationFactor = 1
                }
            }, new CreateTopicsOptions()
            {
                OperationTimeout = TimeSpan.FromSeconds(5),
                RequestTimeout = TimeSpan.FromSeconds(5)
            });
        }

        public void Dispose()
        {
            _adminClient?.Dispose();
        }

        public async Task DeleteTopicAsync(string topicName)
        {
            var topics = new List<string>();
            topics.Add(topicName);
            _adminClient.DeleteTopicsAsync(topics);
        }

    }
}
