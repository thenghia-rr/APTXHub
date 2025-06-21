using APTXHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Helpers
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext appDbContext)
        {
            if(!appDbContext.Users.Any() && !appDbContext.Posts.Any())
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
