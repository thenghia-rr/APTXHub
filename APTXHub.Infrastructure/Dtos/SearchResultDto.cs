using APTXHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Dtos
{
    public class SearchResultDto
    {
        public string Query { get; set; } = string.Empty;
        public List<Post> Posts { get; set; } = [];
        public List<User> Users { get; set; } = [];
    }
}
