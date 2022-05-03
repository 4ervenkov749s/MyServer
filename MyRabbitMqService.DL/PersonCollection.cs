using RabbitMqService.Models;
using System;
using System.Collections.Generic;

namespace MyRabbitMqService.DL
{
    public static class PersonCollection
    {
        public static List<Person> PersonDb = new List<Person>()
        {
            new Person()
            {
                Id = 0,
                Name = "Ivan",
                LastUpdated = DateTime.Now
            },

            new Person()
            {
                Id = 1,
                Name = "Petar",
                LastUpdated = DateTime.Now
            },

            new Person()
            {
                Id = 2,
                Name = "Nikolai",
                LastUpdated = DateTime.Now
            }

        };

    }
}
