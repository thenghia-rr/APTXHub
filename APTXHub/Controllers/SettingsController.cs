using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;

        public SettingsController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 1;
            var userDb = await _userService.GetUser(loggedInUserId);

            return View(userDb);
        }
    }
}
