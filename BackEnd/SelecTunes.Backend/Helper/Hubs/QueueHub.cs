using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper.Hubs
{
    public class QueueHub : Hub
    {
        public async Task UpvoteSong(string SpotifyID)
        {
            await Clients.All.SendAsync("ReceiveUpVote", SpotifyID).ConfigureAwait(false);
        }

        public async Task DownVote(string SpotifyID)
        {
            await Clients.All.SendAsync("ReceiveDownVote", SpotifyID).ConfigureAwait(false);
        }

        public async Task MoveSongToFront(string SpotifyID)
        {
            // TODO Add song to front of queue
            await Clients.All.SendAsync("ReceiveMoveSongToFront", SpotifyID).ConfigureAwait(false);
        }

        public async Task RemoveSong(string SpotifyID)
        {
            // TODO Add song to front of queue
            await Clients.All.SendAsync("ReceiveRemoveSong", SpotifyID).ConfigureAwait(false);
        }
    }
}
