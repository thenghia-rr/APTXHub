using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
    }
}
