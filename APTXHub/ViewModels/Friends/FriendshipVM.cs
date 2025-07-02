using APTXHub.Infrastructure.Models;

namespace APTXHub.ViewModels.Friends
{
    public class FriendshipVM
    {
        public List<Friendship> Friends = [];
        public List<FriendRequest> FriendRequestSent = [];
        public List<FriendRequest> FriendRequestsReceived = [];
    }
}
