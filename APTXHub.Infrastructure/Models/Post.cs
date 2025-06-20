using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NrOfReports { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // Foreign key
        public int UserId { get; set; }

        //Navigation properties
        public User User { get; set; }
    }
}
