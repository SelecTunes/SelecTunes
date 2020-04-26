using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper.Hubs
{
    public class SpotifyHub : Hub
    {
        public async Task UpdateCurrentSong(string image, string song, string artist, string track) // This will be called everytime the player state changes
        {
            Console.WriteLine("Updating Current Song {0} {1} {2} {3}", image, song, artist, track);

            await Clients.All.SendAsync("ReceiveSong", image, song, artist, track).ConfigureAwait(false);
        }
    }
}
