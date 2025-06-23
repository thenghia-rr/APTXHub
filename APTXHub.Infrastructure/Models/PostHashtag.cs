using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Models
{
    public class PostHashtag
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int HashtagId { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
