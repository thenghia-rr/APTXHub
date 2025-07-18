﻿using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Helpers
{
    public static class DbInitializer
    {
        // [2]. Init users and roles of Identity
        public static async Task SeedUsersAndRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //Roles
            if (!roleManager.Roles.Any())
            {
                foreach (var roleName in AppRoles.All)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    }
                }
            }

            //Users with Roles
            // n => !string.IsNullOrEmpty(n.Email)
            if (!userManager.Users.Any())
            {
                var userPassword = "Susu@123";
                var newUser = new User()
                {
                    UserName = "nghiadang@gmail.com",
                    Email = "nghiadang@gmail.com",
                    FullName = "Đặng Thế Nghĩa",
                    ProfilePictureUrl = "https://i.pinimg.com/736x/68/2f/6c/682f6c9e79d3d9b93759216780a86b63.jpg",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, userPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(newUser, AppRoles.User);


                var newAdmin = new User()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FullName = "Admin APTX",
                    ProfilePictureUrl = "https://motgame.vn/stores/news_dataimages/2024/082024/08/17/yua-mikami-gay-tranh-cai-vi-cong-viec-nhay-cam-moi-sau-khi-giai-nghe-20240808172347.jpg?rt=20240808172514",
                    EmailConfirmed = true
                };

                var resultNewAdmin = await userManager.CreateAsync(newAdmin, userPassword);
                if (resultNewAdmin.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, AppRoles.Admin);
            }
        }

        // [1]. Mock user and post data for development purposes
        public static async Task SeedAsync(AppDbContext appDbContext)
        {
            if (!appDbContext.Users.Any() && !appDbContext.Posts.Any())
            {
                var newUser = new User()
                {
                    FullName = "Thế Nghĩa",
                    ProfilePictureUrl = "https://res.cloudinary.com/dh5zl1bel/image/upload/v1721134361/blog_aptx/oxvxjsfid769axsswysz.png"
                };
                await appDbContext.Users.AddAsync(newUser);
                await appDbContext.SaveChangesAsync();

                var newPostWithImage = new Post()
                {
                    Content = "Today was such a wonderful day because I got to meet someone who inspired me so much during my high school years – Khánh Vy. She’s truly lovely and super friendly in real life!",
                    ImageUrl = "https://res.cloudinary.com/dh5zl1bel/image/upload/v1750432233/cloudinary_demo/me_kv_2_pycpik.jpg",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                var newPostWithoutImage = new Post()
                {
                    Content = "This is is my idol, his name is Cristiano Ronaldo. Siuuuuuu...",
                    ImageUrl = "",
                    NrOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,

                    UserId = newUser.Id
                };

                await appDbContext.Posts.AddRangeAsync(newPostWithImage, newPostWithoutImage);
                await appDbContext.SaveChangesAsync();
            }
        }
    }
}
