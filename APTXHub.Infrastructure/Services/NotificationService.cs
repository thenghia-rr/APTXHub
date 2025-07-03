using APTXHub.Infrastructure.Helpers.Constants;
using APTXHub.Infrastructure.Hubs;
using APTXHub.Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext context,
            IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }


        public async Task AddNewNotificationAsync(
            int userId,              // Người nhận
            int fromUserId,          // Người gây ra hành động
            string notificationType,
            string userFullName,     // Tên người gây ra hành động (hiển thị)
            int? postId)
        {
            var newNotification = new Notification()
            {
                UserId = userId,                 // Người nhận
                FromUserId = fromUserId,         // Người thực hiện hành động
                Message = GetPostMessage(notificationType, userFullName),
                Type = notificationType,
                IsRead = false,
                PostId = postId.HasValue ? postId.Value : null,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(newNotification);
            await _context.SaveChangesAsync();

            var notificationNumber = await GetUnreadNotificationsCountAsync(userId);

            await _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", notificationNumber);
        }


        public async Task<int> GetUnreadNotificationsCountAsync(int userId)
        {
            var count = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .CountAsync();

            return count;
        }

        public async Task<List<Notification>> GetNotifications(int userId)
        {
            var allNotifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .Include(n => n.FromUser)
                .OrderBy(n => n.IsRead)
                .ThenByDescending(n => n.DateCreated)
                .ToListAsync();

            return allNotifications;
        }

        private string GetPostMessage(string notificationType, string userFullName)
        {
            var message = "";

            switch (notificationType)
            {
                case NotificationType.Like:
                    message = $"{userFullName} liked your post";
                    break;

                case NotificationType.Favorite:
                    message = $"{userFullName} favorited your post";
                    break;

                case NotificationType.Comment:
                    message = $"{userFullName} added a comment to your post";
                    break;

                case NotificationType.FriendRequest:
                    message = $"{userFullName} added you as friend";
                    break;

                case NotificationType.FriendRequestApproved:
                    message = $"{userFullName} approved your friendship request";
                    break;

                default:
                    message = "";
                    break;
            }

            return message;
        }

        public async Task SetNotificationAsReadAsync(int notificationId)
        {
            var notificationDb = await _context.Notifications
                            .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notificationDb != null)
            {
                notificationDb.DateUpdated = DateTime.UtcNow;
                notificationDb.IsRead = true;

                _context.Notifications.Update(notificationDb);
                await _context.SaveChangesAsync();
            }
        }
    }
}
