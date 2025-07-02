using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Friends;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APTXHub.ViewComponents
{
    public class SuggestedFriendsViewComponent : ViewComponent
    {
        private readonly IFriendsService _friendsService;

        public SuggestedFriendsViewComponent(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var loggedIdUserId = ((ClaimsPrincipal)User).FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(loggedIdUserId!);

            var suggestedFriends = await _friendsService.GetSuggestedFriendsAsync(userId);

            var suggestedFriendsVM = suggestedFriends.Select(n => new UserWithFriendsCountVM()
            {
                UserId = n.User.Id,
                FullName = n.User.FullName,
                ProfilePictureUrl = n.User.ProfilePictureUrl,
                FriendsCount = n.FriendsCount
            }).ToList();
            return View(suggestedFriendsVM);

        }
    }
}
