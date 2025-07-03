using APTXHub.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Services
{
    public interface INotificationService
    {
        Task AddNewNotificationAsync(int userId, int fromUserId, string notificationType, string userFullName, int? postId);
        Task<int> GetUnreadNotificationsCountAsync(int userId);

        Task<List<Notification>> GetNotifications(int userId);
        Task SetNotificationAsReadAsync(int notificationId);
    }
}
 