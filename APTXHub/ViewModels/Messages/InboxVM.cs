using APTXHub.Infrastructure.Models;

namespace APTXHub.ViewModels.Messages
{
    public class InboxVM
    {
        public User CurrentUser { get; set; }
        public List<Message> Conversations { get; set; }
        public User? TargetUser { get; set; }
        public List<Message> Messages { get; set; }
    }
}
