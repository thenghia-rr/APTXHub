using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Dtos
{
    public class ToggleFavoriteResult
    {
        public bool Favorited { get; set; }
        public int TotalFavorites { get; set; }
    }
}
