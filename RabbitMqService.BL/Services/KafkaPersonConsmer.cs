using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using RabbitMqService.BL.DataFlow;
using RabbitMqService.BL.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
 
namespace RabbitMqService.BL.Services
{
    public class KafkaPersonConsumer : IHostedService
    {
       
        private IConsumer<int, byte[]> _consumer;
        private readonly IPersonDataFlow _personDataFlow;

        public KafkaPersonConsumer(IPersonDataFlow personDataFlow)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost",
                AutoCommitIntervalMs = 5000,
                FetchWaitMaxMs = 50,
                GroupId = Guid.NewGuid().ToString(),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
                ClientId = "2",
            };

            _consumer = new ConsumerBuilder<int, byte[]>(config)
             //.SetValueDeserializer(new MsgPackDeserializer<T>())
                .Build();

            _personDataFlow = personDataFlow;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _consumer.Subscribe("test");
            Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);
                        _personDataFlow.SendPerson(cr.Message.Value);
                        //Console.WriteLine($"Consumed message '{cr.Message.Value}' at: '{cr.TopicPartitionOffset}' . ");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

