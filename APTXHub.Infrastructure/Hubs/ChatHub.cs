using APTXHub.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APTXHub.Infrastructure.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task JoinChatGroup(string senderId, string receiverId)
        {
            var groupName = GenerateGroupName(senderId, receiverId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string senderIdStr, string receiverIdStr, string message)
        {
            if (!int.TryParse(senderIdStr, out var senderId) || !int.TryParse(receiverIdStr, out var receiverId))
                return;

            // 👇 Lưu tin nhắn vào database
            var savedMessage = await _messageService.SaveMessageAsync(senderId, receiverId, message);

            var groupName = GenerateGroupName(senderIdStr, receiverIdStr);

            // 👇 Gửi tin nhắn cho tất cả trong nhóm
            await Clients.Group(groupName).SendAsync("ReceiveMessage", senderIdStr, message);
        }

        private string GenerateGroupName(string user1, string user2)
        {
            return "chat_" + string.Join("_", new[] { user1, user2 }.OrderBy(x => x));
        }
    }
}
