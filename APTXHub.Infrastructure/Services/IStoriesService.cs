using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface IStoriesService
    {
       Task<List<Story>> GetAllStoriesAsync();
       Task<Story> CreateStoryAsync(Story story);
    }
}
