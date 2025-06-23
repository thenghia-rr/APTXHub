using APTXHub.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public StoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = 
                await _context.Stories
                    .Include(s => s.User)
                    .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24)) // Gioi han 24h
                    .OrderByDescending(n => n.DateCreated)
                    .ToListAsync();

            return View(allStories);
        }
    }
}