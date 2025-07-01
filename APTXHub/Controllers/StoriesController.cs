using APTXHub.Controllers.Base;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Stories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    [Authorize]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService _storiesService;
        private readonly IFilesService _filesService;

        public StoriesController(
            IStoriesService storiesService
           ,IFilesService filesService)
        {
            _storiesService = storiesService;
            _filesService = filesService;
        }

    

        // [POST]: Create Story
        public async Task<IActionResult> CreateStory(StoryVM storyVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var mediaUploadUrl = await _filesService.UploadMediaAsync(storyVM.Image, MediaFileType.StoryMedia);

            var newStory = new Story
            {
                ImageUrl = mediaUploadUrl, // image/video
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId.Value
            };

          
            await _storiesService.CreateStoryAsync(newStory);

            return RedirectToAction("Index", "Home");
        }
    }
}
