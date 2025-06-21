using System.Diagnostics;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Models;
using APTXHub.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPosts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();
            return View(allPosts);
        }

        //[POST]: Create Post
       [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            int loggedInUserId = 1;

            var newPost = new Post
            {
                Content = post.Content,
                ImageUrl = null, // Assuming no image URL for simplicity
                NrOfReports = 0,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UserId = loggedInUserId
            };

            //Check and save the image
            if (post.Image != null && post.Image.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (post.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await post.Image.CopyToAsync(stream);

                    //Set the URL to the newPost object
                    newPost.ImageUrl = "/images/uploaded/" + fileName;
                }
            }

            await _context.Posts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // [POST] : Like and dislike Post
        [HttpPost]
        public async Task<IActionResult> TogglePostLike([FromBody] PostLikeVM postLike)
        {
            int loggedInUserId = 1;

            var existingLike = await _context.Likes
                 .Where(l => l.PostId == postLike.PostId && l.UserId == loggedInUserId)
                .FirstOrDefaultAsync();

            bool liked;

            if (existingLike != null)
            {
                _context.Likes.Remove(existingLike);
                liked = false;
            }
            else
            {
                var newLike = new Like
                {
                    PostId = postLike.PostId,
                    UserId = loggedInUserId
                };
                await _context.Likes.AddAsync(newLike);
                liked = true;
            }

            await _context.SaveChangesAsync();

            var totalLikes = await _context.Likes
                .CountAsync(l => l.PostId == postLike.PostId);

            return Json(new { liked, totalLikes });
        }

    }
}
