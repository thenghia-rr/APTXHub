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
                var oldImage = user.ProfilePictureUrl;

                user.ProfilePictureUrl = profilePictureUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(oldImage))
                {
                    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var oldFilePath = Path.Combine(rootPath, oldImage.TrimStart('/'));

                    if (File.Exists(oldFilePath))
                    {
                        try
                        {
                            File.Delete(oldFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Lỗi khi xóa ảnh avatar: " + ex.Message);
                        }
                    }
                }
            }
        }
        public async Task UpdateUserCoverPicture(int loggedInUserId, string newCoverPictureUrl)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == loggedInUserId);

            if (user != null)
            {
                // 1. Lưu lại URL cũ
                var oldCoverPictureUrl = user.CoverPictureUrl;

                // 2. Cập nhật URL mới
                user.CoverPictureUrl = newCoverPictureUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // 3. Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(oldCoverPictureUrl))
                {
                    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var oldFilePath = Path.Combine(rootPath, oldCoverPictureUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                        catch (Exception ex)
                        {
                            // Log nếu cần thiết
                            Console.WriteLine("Lỗi khi xóa ảnh bìa cũ: " + ex.Message);
                        }
                    }
                }
            }
        }

        public async Task<List<Post>> GetUserPosts(int userId)
        {
            var allPosts = await _context.Posts
                .Where(n => n.UserId == userId && n.Reports.Count < 5 && !n.IsDeleted)
                .Include(n => n.User)
                .Include(n => n.Likes)
                .Include(n => n.Favorites)
                .Include(n => n.Comments).ThenInclude(n => n.User)
                .Include(n => n.Reports)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();

            return allPosts;
        }

        public async Task<List<User>> GetUserFriends(int userId)
        {
            var friendships = await _context.Friendships
                .Where(f => f.SenderId == userId || f.ReceiverId == userId)
                .Include(f => f.Sender)
                .Include(f => f.Receiver)
                .ToListAsync();

            var friends = friendships
                .Select(f => f.SenderId == userId ? f.Receiver : f.Sender)
                .Distinct()
                .ToList();

            return friends;
        }

    }
}
