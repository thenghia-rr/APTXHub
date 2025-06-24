using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.ViewComponents
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly IStoriesService _storiesService;

        public StoriesViewComponent(AppDbContext context, IStoriesService storiesService)
        {
            _context = context;
            _storiesService = storiesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _storiesService.GetAllStoriesAsync();

            return View(allStories);
        }
    }
}