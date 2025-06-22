using System.Diagnostics;
using APTXHub.Extentions;
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

        // [GET]: Home Page - Show all Posts
        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;
            var allPosts = await _context.Posts
                .Where(n => !n.IsPrivate || n.UserId == loggedInUserId)
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Favorites)
                .Include(p => p.Comments).ThenInclude(c => c.User)
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

        // [POST]: Like and dislike Post
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

        // [POST]: Favorite and unfavorite Post
        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite([FromBody] PostFavoriteVM postFavoriteVM)
        {

            int loggedInUserId = 1;

            var existingFavorited = await _context.Favorites
                .AsNoTracking()
                 .FirstOrDefaultAsync(l => l.PostId == postFavoriteVM.PostId && l.UserId == loggedInUserId);

            bool favorited;

            if (existingFavorited != null)
            {
                _context.Favorites.Remove(existingFavorited);
                favorited = false;
            }
            else
            {
                var newFavorite = new Favorite
                {
                    PostId = postFavoriteVM.PostId,
                    UserId = loggedInUserId,
                    DateCreated = DateTime.UtcNow
                };
                await _context.Favorites.AddAsync(newFavorite);
                favorited = true;
            }

            await _context.SaveChangesAsync();

            var totalFavorited = await _context.Favorites
               .CountAsync(l => l.PostId == postFavoriteVM.PostId);

            return Json(new { favorited, totalFavorited });

        }

        // [POST]: Comment on Post
        [HttpPost]
        public async Task<IActionResult> AddPostComment(PostCommentVM postComment)
        {
            int loggedInUserId = 1;

            //Creat a post object
            var newComment = new Comment()
            {
                UserId = loggedInUserId,
                PostId = postComment.PostId,
                Content = postComment.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };
            await _context.Comments.AddAsync(newComment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // [POST]: Remove comment
        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeComment)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == removeComment.CommentId);

            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // [POST]: Update visibility of Post
        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;

            //get post by id and loggedin user id
            var post = await _context.Posts
                .FirstOrDefaultAsync(l => l.Id == postVisibilityVM.PostId && l.UserId == loggedInUserId);

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
