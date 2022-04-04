using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyRabbitMqService.DL;
using RabbitMqService.BL.Interfaces;
using RabbitMqService.Models;


namespace RabbitMqService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonRepository _personRepository;
        private readonly IKafkaService _kafkaService;

        public PersonController(ILogger<PersonController> logger, IRabbitMqService rabbitMqService, IPersonRepository personRepository, IKafkaService kafkaService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            _personRepository = personRepository;
            _kafkaService = kafkaService;
        }

        [HttpPost("sendPerson")]
        public async Task<IActionResult> SendPerson([FromBody] Person p)
        {
            await _rabbitMqService.SendPersonAsync(p);

            return Ok();
        }


        [HttpPost("InsertInMongo")]
        public async Task<IActionResult> InsertPerson(int id, string name)
        {
            await _personRepository.Add(new Person()
            {
                Id = id,
                Name = name,
                LastUpdated = DateTime.Now
            });

            return Ok();
        }

        [HttpPost("sendtoKafka")]
        public async Task<IActionResult> SendMsgPerson([FromBody] Person p)
        {
            await _kafkaService.SendPersonAsync(p);

            return Ok();
        }
    }
}
