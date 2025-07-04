using APTXHub.Controllers.Base;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APTXHub.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public UsersController(IUserService userService,
            UserManager<User> userManager,
            AppDbContext context)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {

            return View();  
        }

        // [GET]: detail page user
        [HttpGet]
        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
                return NotFound();

            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            string? friendshipStatus = null;

            // 1. Đã là bạn?
            bool isFriend = await _context.Friendships.AnyAsync(f =>
                (f.SenderId == loggedInUserId && f.ReceiverId == userId) ||
                (f.SenderId == userId && f.ReceiverId == loggedInUserId));

            if (isFriend)
            {
                friendshipStatus = "Accepted";
            }
            else
            {
                // 2. Có lời mời chưa xử lý?
                var request = await _context.FriendRequests.FirstOrDefaultAsync(f =>
                    (f.SenderId == loggedInUserId && f.ReceiverId == userId) ||
                    (f.SenderId == userId && f.ReceiverId == loggedInUserId));

                friendshipStatus = request?.Status;
            }

            var userPosts = await _userService.GetUserPosts(userId);
            var userFriends = await _userService.GetUserFriends(userId);

            var userProfileVM = new GetUserProfileVM
            {
                User = user,
                Posts = userPosts,
                Friends = userFriends,
                FriendshipStatus = friendshipStatus
            };

            return View(userProfileVM);
        }


    }
}
