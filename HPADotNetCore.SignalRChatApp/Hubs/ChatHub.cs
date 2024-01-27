using Microsoft.AspNetCore.SignalR;

namespace HPADotNetCore.SignalRChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ServerSendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ClientReceiveMessage", user, message);
        }
    }
}
