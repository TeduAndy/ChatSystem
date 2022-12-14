using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class ChatHub : Hub
    {

        /// <summary>
        /// 傳遞訊息使用
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
