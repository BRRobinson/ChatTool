using ChatTool.Models.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace ChatTool.API.Hubs
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(int chatId, MessageDTO message)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var chatId = httpContext?.Request.Query["chatid"];
            if (!string.IsNullOrWhiteSpace(chatId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId!);
            }
            await base.OnConnectedAsync();
        }
    }
}
