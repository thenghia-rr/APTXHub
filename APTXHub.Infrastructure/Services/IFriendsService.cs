using APTXHub.Infrastructure.Dtos;
using APTXHub.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IFriendsService
    {
        Task SendRequestAsync(int senderId, int receiverId);
        Task UpdateRequestAsync(int requestId, string status);
        Task RemoveFriendAsync(int frienshipId);
        Task<List<UserWithFriendsCountDto>> GetSuggestedFriendsAsync(int userId);

        // Gui yeu cau kb
        Task<List<FriendRequest>> GetSentFriendRequestAsync(int userId);
        // Nhan loi moi kb
        Task<List<FriendRequest>> GetReceivedFriendRequestAsync(int userId);
    }
}
