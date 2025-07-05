using APTXHub.Infrastructure.Models;

namespace APTXHub.ViewModels.Messages
{
    public class ChatVM
    {
        public User CurrentUser { get; set; } // người đang đăng nhập
        public User TargetUser { get; set; } // người đang chat cùng
        public List<Message> Messages { get; set; }
    }
}
