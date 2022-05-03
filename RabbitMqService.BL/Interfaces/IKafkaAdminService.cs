using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.BL.Interfaces
{
    public interface IKafkaAdminService
    {
        Task CreateTopicAsync(string topicName);

        Task DeleteTopicAsync(string topicName);
    }
}
