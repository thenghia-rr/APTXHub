using System.Diagnostics;
using APTXHub.Controllers.Base;
using APTXHub.Data.Helpers;
using APTXHub.Extentions;
using APTXHub.Infrastructure.Hubs;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using APTXHub.Infrastructure.Helpers.Constants;

namespace APTXHub.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IPostService _postService;
        private readonly IHashtagService _hashtagService;
        private readonly IFilesService _filesService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;

        public HomeController(ILogger<HomeController> logger,
            AppDbContext context,
            IPostService postService,
            IHashtagService hashtagService,
            IFilesService filesService,
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _postService = postService;
            _hashtagService = hashtagService;
            _filesService = filesService;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        // [GET]: Home Page - Show all Posts
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
                return RedirectToLogin();

            var allPosts = await _postService.GetAllPostsAsync(loggedInUserId.Value);
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
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null)
                return RedirectToLogin();


            var imageUploadUrl = await _filesService.UploadMediaAsync(post.Image, MediaFileType.PostImage);

            var newPost = new Post
            {
                Content = post.Content,
                ImageUrl = imageUploadUrl,
                NrOfReports = 0,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                UserId = loggedInUserId.Value
            };

            await _postService.CreatePostAsync(newPost);
            await _hashtagService.ProcessHashtagsForNewPostAsync(post.Content, newPost.Id);

            return RedirectToAction("Index");
        }

        // [POST]: Like and dislike Post 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
        {
            var userId = GetUserId();
            var userName = GetUserName();
            if (userId == null) return RedirectToLogin();

            var res = await _postService.TogglePostLikeAsync(postLikeVM.PostId, userId.Value);
            var post = await _postService.GetPostByIdAsync(postLikeVM.PostId);

            // Notification
            if (res.SendNotification && userId != post.UserId)
            {
                await _notificationService
                    .AddNewNotificationAsync(post.UserId, userId.Value, NotificationType.Like, userName!, postLikeVM.PostId);
            }

            return PartialView("Home/_Post", post);
        }

        // [POST]: Favorite and unfavorite Post 
        [HttpPost]
        public async Task<IActionResult> TogglePostFavorite(PostFavoriteVM postFavoriteVM)
        {

            var userId = GetUserId();
            var userName = GetUserName();
            if (userId == null) return RedirectToLogin();

            var res = await _postService.TogglePostFavoriteAsync(postFavoriteVM.PostId, userId.Value);


            var post = await _postService.GetPostByIdAsync(postFavoriteVM.PostId);

            // Notification
            if (res.SendNotification && userId != post.UserId)
            {
                await _notificationService
                    .AddNewNotificationAsync(post.UserId, userId.Value, NotificationType.Favorite, userName!, postFavoriteVM.PostId);
            }

            return PartialView("Home/_Post", post);

        }

        // [POST]: Update visibility of Post
        [HttpPost]
        public async Task<IActionResult> TogglePostVisibility(PostVisibilityVM postVisibilityVM)
        {
            var userId = GetUserId();
            if (userId == null) return RedirectToLogin();

            await _postService.TogglePostVisibilityAsync(postVisibilityVM.PostId, userId.Value);

            return RedirectToAction("Index");
        }

        // [POST]: Comment on Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostComment(PostCommentVM postCommentVM)
        {
            var userId = GetUserId();
            var userName = GetUserName();
            if (userId == null) return RedirectToLogin();

            //Creat a post object
            var newComment = new Comment()
            {
                UserId = userId.Value,
                PostId = postCommentVM.PostId,
                Content = postCommentVM.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _postService.AddPostCommentAsync(newComment);
            var post = await _postService.GetPostByIdAsync(postCommentVM.PostId);

            // Notification
            if (userId != post.UserId)
            {
                await _notificationService
                    .AddNewNotificationAsync(post.UserId, userId.Value, NotificationType.Comment, userName!, postCommentVM.PostId);

            }

            return PartialView("Home/_Post", post);
        }


        // [POST]: Remove comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePostComment(RemoveCommentVM removeCommentVM)
        {
            await _postService.RemovePostCommentAsync(removeCommentVM.CommentId);
            var post = await _postService.GetPostByIdAsync(removeCommentVM.PostId);
            return PartialView("Home/_Post", post);
        }


        // [POST]: Report Post
        [HttpPost]
        public async Task<IActionResult> AddPostReport(PostReportVM postReportVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            await _postService.ReportPostAsync(postReportVM.PostId, loggedInUserId.Value, postReportVM.Reason);
            return RedirectToAction("Index");
        }

        // [POST]: Delete Post
        [HttpPost]
        public async Task<IActionResult> PostRemoveSoft(PostRemoveVM postRemoveVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var post = await _postService.RemovePostSoftAsync(postRemoveVM.PostId, loggedInUserId.Value);

            if (post != null)
            {
                await _hashtagService.ProcessHashtagsForRemovedPostAsync(post);
            }
            return RedirectToAction("Index");
        }
    }
}
