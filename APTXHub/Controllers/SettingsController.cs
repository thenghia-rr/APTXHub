using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Helpers.Enums;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Setttings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APTXHub.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFilesService _filesService;

        public SettingsController(IUserService userService, IFilesService filesService)
        {
            _userService = userService;
            _filesService = filesService;
        }

        //[GET]: show information of user
        public async Task<IActionResult> Index()
        {
            var loggedInUserId = 8;
            var userDb = await _userService.GetUser(loggedInUserId);

            return View(userDb);
        }

        // [POST]: update profile picture
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureVM profilePictureVM)
        {
            var loggedInUser = 1;
            var uploadedProfilePictureUrl = await _filesService.UploadMediaAsync(
                    profilePictureVM.ProfilePictureImage, 
                    MediaFileType.ProfilePicture);

            await _userService.UpdateUserProfilePicture(loggedInUser, uploadedProfilePictureUrl!);

            return RedirectToAction("Index");
        }

        // [POST]: update profile of user
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            return RedirectToAction("Index");
        }


        // [POST]: update password of user
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            return RedirectToAction("Index"); 
        }
    }
}
