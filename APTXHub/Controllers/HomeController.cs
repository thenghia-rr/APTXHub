using System.Diagnostics;
using APTXHub.Data.Helpers;
using APTXHub.Extentions;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IPostService _postService;
        private readonly IHashtagService _hashtagService;
        private readonly IFilesService _filesService;

        public HomeController(ILogger<HomeController> logger,
            AppDbContext context,
            IPostService postService,
            IHashtagService hashtagService,
            IFilesService filesService)
        {
            _logger = logger;
            _context = context;
            _postService = postService;
            _hashtagService = hashtagService;
            _filesService = filesService;
        }

        // [GET]: Home Page - Show all Posts
        public async Task<IActionResult> Index()    
        {
            int loggedInUserId = 1;

            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId);
            return View(allPosts);
        }


        // [GET]: Post detail Page
        // Da duoc custom route pattern: /post/{id}
        public async Task<IActionResult> Details(int postid)
        {
            var post = await _postService.GetPostByIdAsync(postid);

            if (post == null)
            {
                return NotFound(); // trả về 404 nếu không có post
            }

            return View(post);
        }
        //[POST]: Create Post
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostVM post)
        {
            //Get the logged in user
            int loggedInUserId = 1;
            var imageUploadUrl = await _filesService.UploadMediaAsync(post.Image, MediaFileType.PostImage);

            var newPost = new Post
            {
                Content = post.Content,
                ImageUrl = imageUploadUrl,
                NrOfReports = 0,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UserId = loggedInUserId
            };

            await _postService.CreatePostAsync(newPost);
            await _hashtagService.ProcessHashtagsForNewPostAsync(post.Content, newPost.Id);

            return RedirectToAction("Index");
        }

        // [POST]: Like and dislike Post 
        [HttpPost]
        public async Task<IActionResult> TogglePostLike([FromBody] PostLikeVM postLike)
        {
            int loggedInUserId = 1;

            var result = await _postService.TogglePostLikeAsync(postLike.PostId, loggedInUserId);

            return Json(new { liked = result.Liked, totalLikes = result.TotalLikes });
        }

        // [POST]: Favorite and unfavorite Post 
        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite([FromBody] PostFavoriteVM postFavoriteVM)
        {

            int loggedInUserId = 1;

            var res = await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, loggedInUserId);

            return Json(new { favorited = res.Favorited, totalFavorited = res.TotalFavorites });

        }

        // [POST]: Update visibility of Post
        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            int loggedInUserId = 1;

            await _postService.TogglePostVisibilityAsync(postVisibilityVM.PostId, loggedInUserId);

            return RedirectToAction("Index");
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

            await _postService.AddPostCommentAsync(newComment);

            return RedirectToAction("Index");
        }


        // [POST]: Remove comment
        [HttpPost]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeComment)
        {
            await _postService.RemovePostCommentAsync(removeComment.CommentId);
            return RedirectToAction("Index");
        }

        
        // [POST]: Report Post
        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            int loggedInUserId = 1;
           
            await _postService.ReportPostAsync(postReportVM.PostId, loggedInUserId, postReportVM.Reason);
            return RedirectToAction("Index");
        }

        // [POST]: Delete Post
        [HttpPost]
        public async Task<IActionResult> PostRemoveSoft(PostRemoveVM postRemoveVM)
        {
            int loggedInUserId = 1;

            var post = await _postService.RemovePostSoftAsync(postRemoveVM.PostId, loggedInUserId);

            if(post != null)
            {
                await _hashtagService.ProcessHashtagsForRemovedPostAsync(post);
            }
            return RedirectToAction("Index");
        }
    }
}
