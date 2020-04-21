using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper.Hubs
{
    public class QueueHub : Hub
    {
        public const int VotesToTriggerAction = 2;

        private readonly IDistributedCache _cache;

        private readonly UserManager<User> _userManager;

        private readonly ApplicationContext _context;

        private readonly PlaybackHelper _playback;

        public QueueHub(IDistributedCache cache, UserManager<User> manager, ApplicationContext context, PlaybackHelper playback)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _userManager = manager ?? throw new ArgumentNullException(nameof(manager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _playback = playback ?? throw new ArgumentNullException(nameof(playback));
        }

        public async Task JoinQueue(string queueName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, queueName).ConfigureAwait(false);
        }

        public async Task LeaveQueue(string queueName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, queueName).ConfigureAwait(false);
        }

        public async Task UpvoteSong(string spotifyId)
        {
            User user = await _userManager.GetUserAsync(Context.User).ConfigureAwait(false);

            if (user == null)
            {
                // TODO: Log error that user is nil.
                return;
            }

            Party party = _context.Parties.Find(user.PartyId);

            if (party == null)
            {
                // TODO: Log error that user is not in party.
                return;
            }

            string partyId = party.JoinCode;

            byte[] v = await _cache.GetAsync($"$votes:${partyId}:${spotifyId}").ConfigureAwait(false);
            if (v == null)
            {
                v = Encoding.UTF8.GetBytes("0");
            }

            int votes = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(v));

            votes++;

            await Clients.All.SendAsync("ReceiveUpvote", spotifyId, votes).ConfigureAwait(false);

            if (votes >= VotesToTriggerAction)
            {
                await _playback.SendToSpotifyQueue(party.PartyHost, spotifyId).ConfigureAwait(false);
                await MoveSongToFront(spotifyId).ConfigureAwait(true);
                await _cache.RemoveAsync($"$votes:${partyId}:${spotifyId}").ConfigureAwait(false); // Remove it from the voteable pool, as it is no longer votable.

                return;
            }

            await _cache.SetStringAsync($"$votes:${partyId}:${spotifyId}", JsonConvert.SerializeObject(votes)).ConfigureAwait(false);
        }

        public async Task DownvoteSong(string spotifyId)
        {
            User user = await _userManager.GetUserAsync(Context.User).ConfigureAwait(false);

            if (user == null)
            {
                // TODO: Log error that user is nil.
                return;
            }

            Party party = _context.Parties.Find(user.PartyId);

            if (party == null)
            {
                // TODO: Log error that user is not in party.
                return;
            }

            string partyId = party.JoinCode;

            byte[] v = await _cache.GetAsync($"$votes:${partyId}:${spotifyId}").ConfigureAwait(false);
            if (v == null)
            {
                v = Encoding.UTF8.GetBytes("0");
            }

            int votes = JsonConvert.DeserializeObject<int>(Encoding.UTF8.GetString(v));

            votes--;

            await Clients.All.SendAsync("ReceiveDownvote", spotifyId, votes).ConfigureAwait(false);

            if (votes <= -VotesToTriggerAction)
            {
                await RemoveSong(spotifyId).ConfigureAwait(true);
                await _cache.RemoveAsync($"$votes:${partyId}:${spotifyId}").ConfigureAwait(false);

                return;
            }

            await _cache.SetStringAsync($"$votes:${partyId}:${spotifyId}", JsonConvert.SerializeObject(votes)).ConfigureAwait(false);
        }

        public async Task MoveSongToFront(string spotifyId)
        {
            User user = await _userManager.GetUserAsync(Context.User).ConfigureAwait(false);

            if (user == null)
            {
                // TODO: Log error that user is nil.
                return;
            }

            Party party = _context.Parties.Find(user.PartyId);

            if (party == null)
            {
                // TODO: Log error that user is not in party.
                return;
            }

            string partyId = party.JoinCode;

            byte[] queue = await _cache.GetAsync($"$queue:${partyId}").ConfigureAwait(false);
            if (queue == null)
            {
                return;
            }

            Queue<Song> q = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(queue));

            Song s = RemoveSongFromQueue(ref q, spotifyId);

            await _cache.SetStringAsync($"$queue:${partyId}", JsonConvert.SerializeObject(q)).ConfigureAwait(false);

            byte[] locked = await _cache.GetAsync($"$locked:${partyId}").ConfigureAwait(false);
            if (locked == null)
            {
                await _cache.SetStringAsync($"$locked:${party.JoinCode}", JsonConvert.SerializeObject(new Queue<Song>())).ConfigureAwait(false);
                locked = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new Queue<Song>()));
            }

            Queue<Song> lockedIn = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(locked));

            lockedIn.Enqueue(s);

            await _cache.SetStringAsync($"$locked:${partyId}", JsonConvert.SerializeObject(lockedIn)).ConfigureAwait(false);

            await Clients.All.SendAsync("ReceiveMoveSongToFront", spotifyId).ConfigureAwait(false);
        }

        public async Task UpdateCurrentSong(string image, string song, string artist, string track) // This will be called everytime the player state changes
        {
            Console.WriteLine("Updating Current Song {0} {1} {2} {3}", image, song, artist, track);

            await Clients.All.SendAsync("ReceiveSong", image, song, artist, track).ConfigureAwait(false);
        }

        public async Task RemoveSong(string spotifyId)
        {
            User user = await _userManager.GetUserAsync(Context.User).ConfigureAwait(false);

            if (user == null)
            {
                // TODO: Log error that user is nil.
                return;
            }

            Party party = _context.Parties.Find(user.PartyId);

            if (party == null)
            {
                // TODO: Log error that user is not in party.
                return;
            }

            string partyId = party.JoinCode;

            byte[] queue = await _cache.GetAsync($"$queue:${partyId}").ConfigureAwait(false);
            if (queue == null)
            {
                return;
            }

            Queue<Song> q = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(queue));

            RemoveSongFromQueue(ref q, spotifyId);

            await _cache.SetStringAsync($"$queue:${partyId}", JsonConvert.SerializeObject(q)).ConfigureAwait(false);

            await Clients.All.SendAsync("ReceiveRemoveSong", spotifyId).ConfigureAwait(false);
        }

        private static Song RemoveSongFromQueue(ref Queue<Song> songs, string spotifyId)
        {
            Console.WriteLine(JsonConvert.SerializeObject(songs));

            List<Song> list = BuildSongList(songs);

            Song s = list.Find(song => song.Id == spotifyId);

            list.RemoveAll(song => song.Id == spotifyId);

            Console.WriteLine(JsonConvert.SerializeObject(list));

            songs = BuildSongQueue(list);

            return s;
        }

        private static List<Song> BuildSongList(Queue<Song> queue)
        {
            List<Song> list = new List<Song>();

            while (queue.Count != 0)
            {
                list.Add(queue.Dequeue());
            }

            return list;
        }

        private static Queue<Song> BuildSongQueue(List<Song> list)
        {
            Queue<Song> queue = new Queue<Song>();

            list.ForEach(queue.Enqueue);

            return queue;
        }
    }
}
