using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Dtos
{
    public class ToggleLikeResult
    {
        public bool Liked { get; set; }
        public int TotalLikes { get; set; }
    }
}
