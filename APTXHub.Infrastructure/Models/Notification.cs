using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Người nhận
        public User User { get; set; }  // Navigation property

        public int? FromUserId { get; set; }  // Người tạo hành động (like, comment,...)
        public User FromUser { get; set; } = default!;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }

        public string Type { get; set; } = string.Empty;

        public int? PostId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
