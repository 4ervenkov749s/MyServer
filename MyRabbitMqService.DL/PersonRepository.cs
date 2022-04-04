using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMqService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRabbitMqService.DL
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMongoCollection<Person> _collection;

        public PersonRepository(IOptions<MongoDbConfiguration> mongoOptions)
        {
            var client = new MongoClient(mongoOptions.Value.ConnectionString);
            var database = client.GetDatabase(mongoOptions.Value.DatabaseName);
            _collection = database.GetCollection<Person>("Persons");
        }

        public async Task Add(Person p)
        {
            await _collection.InsertOneAsync(p);
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            var result = await _collection.FindAsync(p => true);

            return result.ToList();
        }

        public async Task<IEnumerable<Person>> GetAllByDate(DateTime lastUpdated)
        {
            var result = await _collection.FindAsync(p => p.LastUpdated >= lastUpdated);

            return result.ToList();
        }
    }
}
