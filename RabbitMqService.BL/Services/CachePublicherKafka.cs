using Microsoft.Extensions.Hosting;
using MyRabbitMqService.DL;
using RabbitMqService.BL.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMqService.BL.Services
{
    public class CachePublisherKafka : IHostedService, IDisposable
    {
        private Timer _timer;
        private DateTime _lastPublished = DateTime.Now;
        private readonly IPersonRepository _personRepository;
        private readonly IKafkaService _kafkaService;

        public CachePublisherKafka(IPersonRepository personRepository, IKafkaService kafkaService)
        {
            _personRepository = personRepository;
            _kafkaService = kafkaService;

            //personRepository.Add(new Person()
            //{
            //    Id = 1,
            //    Name = "Apostol",
            //    LastUpdated = DateTime.Now.AddSeconds(10)
            //});

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;

        }

        private async void DoWork(object state)
        {
            var persons = await _personRepository.GetAllByDate(_lastPublished.ToUniversalTime());

            DateTime lastDate;

            foreach (var person in persons)
            {
                _kafkaService.SendPersonAsync(person);
            }
            _lastPublished = DateTime.Now;

            //foreach (var person in persons.OrderBy(x => x.LastUpdated))
            //{
            //    _rabbitMq.SendPersonAsync(person);
            //}
            //_lastPublished = persons.Last().LastUpdated;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }


        public void Dispose()
        {
            _timer?.DisposeAsync();
        }
    }
}
