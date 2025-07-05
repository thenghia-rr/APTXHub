using APTXHub.Controllers.Base;
using APTXHub.Infrastructure;
using APTXHub.Infrastructure.Models;
using APTXHub.Infrastructure.Services;
using APTXHub.ViewModels.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APTXHub.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMessageService _messageService;

        public MessagesController(AppDbContext context,
            UserManager<User> userManager,
            IMessageService messageService)
        {
            _context = context;
            _userManager = userManager;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? chatWithUserId)
        {
            var currentUserId = GetUserId();

            // 1. Lấy tất cả tin nhắn liên quan đến current user (có kèm thông tin người gửi & nhận)
            var allMessages = await _context.Messages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            // 2. Nhóm theo mỗi người đã chat (người còn lại, không phải current user)
            var conversations = allMessages
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => g.First()) // lấy tin nhắn mới nhất từ mỗi cuộc trò chuyện
                .ToList();

            // 3. Nếu có chọn người để chat thì load toàn bộ tin nhắn 2 chiều
            List<Message> messages = new();
            User? targetUser = null;

            if (chatWithUserId.HasValue)
            {
                targetUser = await _userManager.FindByIdAsync(chatWithUserId.Value.ToString());

                if (targetUser != null)
                {
                    messages = allMessages
                        .Where(m =>
                            (m.SenderId == currentUserId && m.ReceiverId == targetUser.Id) ||
                            (m.SenderId == targetUser.Id && m.ReceiverId == currentUserId))
                        .OrderBy(m => m.SentAt)
                        .ToList();
                }
            }

            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString());

            var viewModel = new InboxVM
            {
                CurrentUser = currentUser!,
                Conversations = conversations,
                TargetUser = targetUser,
                Messages = messages
            };

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Chat(int userId)
        {
            var currentUserId = GetUserId();

            // 1. Kiểm tra bạn bè
            bool isFriend = await _context.Friendships.AnyAsync(f =>
                (f.SenderId == currentUserId && f.ReceiverId == userId) ||
                (f.SenderId == userId && f.ReceiverId == currentUserId));

            if (!isFriend) return Forbid();

            // 2. Lấy thông tin người nhận và người gửi
            var targetUser = await _userManager.FindByIdAsync(userId.ToString());
            var currentUser = await _userManager.FindByIdAsync(currentUserId.ToString()!);

            if (targetUser == null || currentUser == null) return NotFound();

            // 3. Lấy danh sách tin nhắn giữa 2 người
            var messages = await _context.Messages
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                    (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            // 4. Trả về View
            return View(new ChatVM
            {
                CurrentUser = currentUser,
                TargetUser = targetUser,
                Messages = messages
            });
        }

    }
}
