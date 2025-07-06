using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class Reel
    {
        public int Id { get; set; }
        public string VideoUrl { get; set; } = null!;
        public string? Caption { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Liên kết với User
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
