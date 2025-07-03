﻿using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Models;
using APTXHub.ViewModels.Auth;
using APTXHub.ViewModels.Setttings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APTXHub.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        // [POST]: Sign in
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password. Please, try again.");
                return View(loginVM);
            }

            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            if (!existingUserClaims.Any(c => c.Type == CustomClaim.FullName))
                await _userManager.AddClaimAsync(user, new Claim(CustomClaim.FullName, user.FullName));

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginVM.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (User.IsInRole(AppRoles.Admin))
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(loginVM);
        }

        // [GET]: view for user registration
        public IActionResult Register()
        {
            return View();
        }

        // [POST]: Create a new user account
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var newUser = new User
            {
                FullName = $"{registerVM.LastName} {registerVM.FirstName}",
                Email = registerVM.Email,
                UserName = registerVM.Email
            };

            var existingUser = await _userManager.FindByEmailAsync(registerVM.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(registerVM);
            }

            var result = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, AppRoles.User);
                await _userManager.AddClaimAsync(newUser, new Claim(CustomClaim.FullName, newUser.FullName));
                await _signInManager.SignInAsync(newUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerVM);
        }

        // [POST]: update password of user
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM updatePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["PasswordError"] = "Please enter full information to input!";
                TempData["ActiveTab"] = "Password";

                return RedirectToAction("Index", "Settings");
            }

            if (updatePasswordVM.NewPassword != updatePasswordVM.ConfirmPassword)
            {
                TempData["PasswordError"] = "Passwords do not match";
                TempData["ActiveTab"] = "Password";

                return RedirectToAction("Index", "Settings");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(loggedInUser!, updatePasswordVM.CurrentPassword);

            if (!isCurrentPasswordValid)
            {
                TempData["PasswordError"] = "Current password is invalid";
                TempData["ActiveTab"] = "Password";
                return RedirectToAction("Index", "Settings");
            }

            var result = await _userManager.ChangePasswordAsync(loggedInUser!, updatePasswordVM.CurrentPassword, updatePasswordVM.NewPassword);

            if (result.Succeeded)
            {
                TempData["PasswordSuccess"] = "Password updated successfully";
                TempData["ActiveTab"] = "Password";
                await _signInManager.RefreshSignInAsync(loggedInUser!);
            }

            return RedirectToAction("Index", "Settings");
        }

        // [POST]: update profile of user
        public async Task<IActionResult> UpdateProfile(UpdateProfileVM profileVM)
        {
            if (!ModelState.IsValid)
            {
                TempData["UserProfileError"] = "Please enter full information to input!";
                TempData["ActiveTab"] = "Profile";
                return RedirectToAction("Index", "Settings");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            if (loggedInUser == null)
                return RedirectToAction("Login");

            loggedInUser.FullName = profileVM.FullName;
            loggedInUser.UserName = profileVM.UserName;
            loggedInUser.Bio = profileVM.Bio;

            var result = await _userManager.UpdateAsync(loggedInUser);
            if (!result.Succeeded)
            {
                TempData["UserProfileError"] = "User profile could not be updated";
                TempData["ActiveTab"] = "Profile";
            }
            else
            {
                TempData["UserProfileSuccess"] = "User profile updated successfully";
                TempData["ActiveTab"] = "Profile";
            }

            await _signInManager.RefreshSignInAsync(loggedInUser);
            return RedirectToAction("Index", "Settings");
        }

        // [GET]: External login (Google, etc.)
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // [GET]: Callback for external login
        public async Task<IActionResult> ExternalLoginCallback()
        {
            var info = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (info == null)
                return RedirectToAction("Login");

            var email = info?.Principal?.FindFirstValue(ClaimTypes.Email);
           
            var user = await _userManager.FindByEmailAsync(email!);

            if (user == null)
            {
                var newUser = new User()
                {
                    Email = email,
                    UserName = email,
                    FullName = info?.Principal?.FindFirstValue(ClaimTypes.Name) ?? "Anonymous",
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, AppRoles.User);
                    await _userManager.AddClaimAsync(newUser, new Claim(CustomClaim.FullName, newUser.FullName));
                    await _signInManager.SignInAsync(newUser, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
            }

            await _signInManager.SignInAsync(user!, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }



        // [POST]: Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
