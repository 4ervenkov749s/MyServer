using RabbitMqService.Models;
using System.Threading.Tasks;

namespace RabbitMqService.BL.Interfaces
{
    public interface IKafkaService
    {
        Task SendPersonAsync(Person p);

    }
}
