using APTXHub.Controllers.Base;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Friends;
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
    }
}
