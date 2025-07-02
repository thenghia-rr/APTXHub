using APTXHub.Controllers.Base;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Setttings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APTXHub.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IFilesService _filesService;
        private readonly UserManager<User> _userManager;

        public SettingsController(
            IUserService userService, 
            IFilesService filesService,
            UserManager<User> userManager)
        {
            _userService = userService;
            _filesService = filesService;
            _userManager = userManager;
        }

        //[GET]: show information of user
        public async Task<IActionResult> Index()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            return View(loggedInUser);
        }

        // [POST]: update profile picture
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateUserPictureVM updateUserPictureVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var uploadedProfilePictureUrl = await _filesService.UploadMediaAsync(
                    updateUserPictureVM.ProfilePictureImage, 
                    MediaFileType.ProfilePicture);

            await _userService.UpdateUserProfilePicture(loggedInUserId.Value, uploadedProfilePictureUrl!);

            return RedirectToAction("Index");
        }

        // [POST]: update cover picture
        [HttpPost]
        public async Task<IActionResult> UpdateCoverPicture(UpdateUserPictureVM updateUserPictureVM)
        {
            var loggedInUserId = GetUserId();
            if (loggedInUserId == null) return RedirectToLogin();

            var uploadedCoverPictureUrl = await _filesService.UploadMediaAsync(
                    updateUserPictureVM.CoverImage,
                    MediaFileType.CoverImage);

            await _userService.UpdateUserCoverPicture(loggedInUserId.Value, uploadedCoverPictureUrl!);

            return RedirectToAction("Index");
        }

    }
}
