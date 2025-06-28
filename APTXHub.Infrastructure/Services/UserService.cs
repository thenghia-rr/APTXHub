using APTXHub.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(n => n.Id == loggedInUserId) ?? new User();
        }

        public async Task UpdateUserProfilePicture(int loggedInUserId, string profilePictureUrl)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == loggedInUserId);

            if(user != null)
            {
                user.ProfilePictureUrl = profilePictureUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
