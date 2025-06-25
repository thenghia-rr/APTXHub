using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IPostService _postService;

        public FavoritesController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1;

            var allFavoritesPosts = await _postService
                .GetAllFavoritePostsAsync(loggedInUserId);

            return View(allFavoritesPosts);
        }
    }
}
