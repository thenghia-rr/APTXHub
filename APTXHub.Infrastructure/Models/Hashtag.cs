using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{

    public class Hashtag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    }
}
