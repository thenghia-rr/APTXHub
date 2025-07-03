using APTXHub.Controllers.Base;
using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Friends;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    [Authorize]
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;
        private readonly INotificationService _notificationService;

        public FriendsController(IFriendsService friendsService,
            INotificationService notificationService)
        {
            _friendsService = friendsService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (!userId.HasValue) RedirectToLogin();

            var friendsData = new FriendshipVM()
            {
                Friends = await _friendsService.GetFriendsAsync(userId.Value),
                FriendRequestSent = await _friendsService.GetSentFriendRequestAsync(userId.Value),
                FriendRequestsReceived = await _friendsService.GetReceivedFriendRequestAsync(userId.Value)
            };

            return View(friendsData);
        }

        // [POST]: Send fr request
        [HttpPost]
        public async Task<IActionResult> SendFriendRequest(int receiverId)
        {
            var userId = GetUserId();
            var userName = GetUserName();
            if (!userId.HasValue) RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);

            await _notificationService.AddNewNotificationAsync(receiverId, userId.Value, NotificationType.FriendRequest, userName!, null);

            return RedirectToAction("Index", "Home");
        }

        // [POST]: Cancel - Accept - Reject
        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {

            var userId = GetUserId();
            var userName = GetUserName();
            if (!userId.HasValue) RedirectToLogin();

            var request = await _friendsService.UpdateRequestAsync(requestId, status);

            if (status == FriendshipStatus.Accepted)
                await _notificationService.AddNewNotificationAsync(request.SenderId, userId.Value, NotificationType.FriendRequestApproved, userName!, null);

            return RedirectToAction("Index");
        }

        // [POST]: Remove fr
        [HttpPost]  
        public async Task<IActionResult> RemoveFriend(int friendshipId)
        {
            await _friendsService.RemoveFriendAsync(friendshipId);  
            return RedirectToAction("Index");
        }
    }
}
