using APTXHub.Controllers.Base;
using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // [POST]: Get total unread notifications count
        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var count = await _notificationService.GetUnreadNotificationsCountAsync(userId.Value);
            return Json(count);
        }

        // [GET]: Get all notifications
        public async Task<IActionResult> GetNotifications()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var notifications = await _notificationService.GetNotifications(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);
        }

        // [POST]: Set a notification as read
        [HttpPost]
        public async Task<IActionResult> SetNotificationAsRead(int notificationId)
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            await _notificationService.SetNotificationAsReadAsync(notificationId);

            var notifications = await _notificationService.GetNotifications(userId.Value);
            return PartialView("Notifications/_Notifications", notifications);
        }
    }
}
