using Microsoft.Extensions.Hosting;
using MyRabbitMqService.DL;
using RabbitMqService.BL.Interfaces;
using RabbitMqService.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace RabbitMqService.BL.Services
{
    public class CachePublisher : IHostedService, IDisposable
    {
        private Timer _timer;
        private DateTime _lastPublished = DateTime.Now;
        private readonly IPersonRepository _personRepository;
        private readonly IRabbitMqService _rabbitMq;

        public CachePublisher(IPersonRepository personRepository, IRabbitMqService rabbitMq)
        {
            _personRepository = personRepository;
            _rabbitMq = rabbitMq;

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
                _rabbitMq.SendPersonAsync(person);
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
