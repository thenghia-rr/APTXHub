using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }

        // Navigation properties
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
