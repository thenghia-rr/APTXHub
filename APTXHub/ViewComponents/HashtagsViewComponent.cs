﻿using APTXHub.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.ViewComponents
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HashtagsViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var oneWeekAgoNow = DateTime.UtcNow.AddDays(-7); 

            var top3Hashtags = await _context.Hashtags
                .Where(h => h.DateCreated >= oneWeekAgoNow && h.Count > 0)
                .OrderByDescending(n => n.Count)
                .Take(3)
                .ToListAsync();

            return View(top3Hashtags);
        }
    }
}
