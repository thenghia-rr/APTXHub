using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APTXHub.Controllers
{
    [Authorize()]
    public class FavoritesController : Controller
    {
        private readonly IPostService _postService;

        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allFavoritesPosts = await _postService
                .GetAllFavoritePostsAsync(int.Parse(loggedInUserId!));

            return View(allFavoritesPosts);
        }
    }
}
