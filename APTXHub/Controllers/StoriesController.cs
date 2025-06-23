using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Models;
using APTXHub.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    public class StoriesController : Controller
    {
        private readonly AppDbContext _context;

        public StoriesController(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var allStories =
        //        await _context.Stories.Include(s => s.UserId).ToListAsync();
        //    return View(allStories);
        //}   

        // [POST]: Create Story
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            int loggedInUserId = 1;

            var newStory = new Story
            {
                //ImageUrl = null,
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

          
            if (storyVM.Image != null && storyVM.Image.Length > 0)
            {
                var contentType = storyVM.Image.ContentType.ToLower();
                var ext = Path.GetExtension(storyVM.Image.FileName).ToLower();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".webm", ".ogg" };

                if ((contentType.StartsWith("image/") || contentType.StartsWith("video/")) && allowedExtensions.Contains(ext))
                {
                    string folder = contentType.StartsWith("video/") ? "videos/stories" : "images/stories";
                    string fileName = Guid.NewGuid() + ext;

                    string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
                    Directory.CreateDirectory(rootPath);

                    string filePath = Path.Combine(rootPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await storyVM.Image.CopyToAsync(stream);

                    newStory.ImageUrl = $"/{folder}/{fileName}";
                }
            }

            await _context.Stories.AddAsync(newStory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
