using RabbitMqService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRabbitMqService.DL
{
    public interface IPersonRepository
    {
        Task Add(Person p);

        Task<IEnumerable<Person>> GetAll();

        Task<IEnumerable<Person>> GetAllByDate(DateTime lastAdded);
    }
}
 