using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class StoriesSevice : IStoriesService
    {
        private readonly AppDbContext _context;

        public StoriesSevice(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Story>> GetAllStoriesAsync()
        {
            var allStories =
               await _context.Stories
                   .Include(s => s.User)
                   .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24) || n.Id == 9 || n.Id == 3) // Gioi han 24h
                   .OrderByDescending(n => n.DateCreated)
                   .ToListAsync();

            return allStories;
        }

        public async Task<Story> CreateStoryAsync(Story story)
        {
            
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }

    }
}
