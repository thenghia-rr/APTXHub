using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Stories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APTXHub.Controllers
{
    public class StoriesController : Controller
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
            int loggedInUserId = 1;
            var mediaUploadUrl = await _filesService.UploadMediaAsync(storyVM.Image, MediaFileType.StoryMedia);

            var newStory = new Story
            {
                ImageUrl = mediaUploadUrl, // image/video
                DateCreated = DateTime.UtcNow,
                IsDeleted = false,
                UserId = loggedInUserId
            };

          
            await _storiesService.CreateStoryAsync(newStory);

            return RedirectToAction("Index", "Home");
        }
    }
}
