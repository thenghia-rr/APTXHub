using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        //Foreign keys
        public int PostId { get; set; }
        public int UserId { get; set; }

        // ------ Quan he đệ quy (comment nested)
        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        // ------------------------------------------------------------------------

        // Navigation properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
