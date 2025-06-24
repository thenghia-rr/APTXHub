using APTXHub.Infrastructure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IHashtagService
    {
        Task ProcessHashtagsForNewPostAsync(string content, int newPostId);
        Task ProcessHashtagsForRemovedPostAsync(Post post);
    }
}
