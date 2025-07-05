using APTXHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IMessageService
    {
        Task<List<Message>> GetMessagesAsync(int userId1, int userId2);
        Task<Message> SaveMessageAsync(int senderId, int receiverId, string content);

    }
}
