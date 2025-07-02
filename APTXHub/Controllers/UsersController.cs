using APTXHub.Controllers.Base;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {

            return View();  
        }

        public async Task<IActionResult> Details(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.IsDeleted)
            {
                return NotFound();
            }

            var userPosts = await _userService.GetUserPosts(userId);

            var userProfileVM = new GetUserProfileVM
            {
                User = user,
                Posts = userPosts
            };

            return View(userProfileVM);
        }
    }
}
