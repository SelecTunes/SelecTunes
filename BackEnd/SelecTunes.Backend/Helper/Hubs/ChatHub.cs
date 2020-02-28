using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SelecTunes.Backend.Helper.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(string chatName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatName).ConfigureAwait(false);
        }

        public async Task LeaveChat(string chatName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName).ConfigureAwait(false);
        }
    }
}
