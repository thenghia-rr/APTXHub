using APTXHub.Infrastructure.Models;

namespace APTXHub.ViewModels.Users
{
    public class GetUserProfileVM
    {
        public User User { get; set; }
        public List<Post> Posts { get; set; } = [];

        // Trạng thái mối quan hệ hiện tại với user đang login
        public string? FriendshipStatus { get; set; }

    }
}
