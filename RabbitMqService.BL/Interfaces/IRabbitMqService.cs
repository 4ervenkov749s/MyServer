using System.Threading.Tasks;
using RabbitMqService.Models;

namespace RabbitMqService.BL.Interfaces
{
    public interface IRabbitMqService
    {
        Task SendPersonAsync(Person p);
    }
}
