using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly UserManager<User> _userManager;

        public AdminController(IAdminService adminService,
            UserManager<User> userManager)
        {
            _adminService = adminService;
            _userManager = userManager;
        }

        // [GET]: Show all reported posts
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var reportPosts = await _adminService.GetReportedPostsAsync();

            return View(reportPosts);
        }

        // [POST]: Approve a report for post
        [HttpPost]
        public async Task<IActionResult> ApproveReport(int postId)
        {
            await _adminService.ApproveReportAsync(postId);
            return RedirectToAction("Index");
        }

        // [POST]: Reject a report for post
        [HttpPost]
        public async Task<IActionResult> RejectReport(int postId)
        {
            await _adminService.RejectReportAsync(postId);
            return RedirectToAction("Index");
        }

        // [GET]: Danh sách user có role == User
        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(AppRoles.User);

            return View(usersInRole);
        }

        // [POST]: Nâng cấp tài khoản
        [HttpPost]
        public async Task<IActionResult> ToggleVerifyUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return NotFound();

            user.IsVerified = !user.IsVerified; // Toggle trạng thái
            await _userManager.UpdateAsync(user);

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDeleteUser(int userId)
        {
            var user = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            user.IsDeleted = !user.IsDeleted;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("ManageUsers"); 
        }
    }
}
