using RabbitMqService.BL.Interfaces;
using RabbitMqService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace RabbitMqService.BL.Services
{
    public class RabbitMqService5 : IRabbitMqService, IDisposable
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public RabbitMqService5()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("person", ExchangeType.Direct, durable: true);

            _channel.QueueDeclare("person", durable: true, exclusive: false, autoDelete: false);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        public async Task SendPersonAsync(Person p)
        {
            await Task.Factory.StartNew(() =>
            {
                var body = MessagePackSerializer.Serialize(p);

                _channel.BasicPublish("", "person", body: body);
            });
        }
    }
}
