using ChatTool.Models.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace ChatTool.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendChat(int chatId, ChatDTO message)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveChat", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var chatId = httpContext?.Request.Query["chatId"];
            if (!string.IsNullOrWhiteSpace(chatId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId!);
            }
            await base.OnConnectedAsync();
        }
    }
}
