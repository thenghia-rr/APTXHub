using APTXHub.Infrastructure.Models;

namespace APTXHub.ViewModels.Admin
{
    public class PostReportAdminVM
    {
        public List<Post> Posts { get; set; } = [];
        public string Reason { get; set; } = string.Empty;
    }
}
