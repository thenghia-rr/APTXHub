using APTXHub.Controllers.Base;
using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Friends;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    public class FriendsController : BaseController
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
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
            if (!userId.HasValue) RedirectToLogin();

            await _friendsService.SendRequestAsync(userId.Value, receiverId);
            return RedirectToAction("Index", "Home");
        }

        // [POST]: Cancel - Accept - Reject
        [HttpPost]
        public async Task<IActionResult> UpdateFriendRequest(int requestId, string status)
        {
            await _friendsService.UpdateRequestAsync(requestId, status);
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
