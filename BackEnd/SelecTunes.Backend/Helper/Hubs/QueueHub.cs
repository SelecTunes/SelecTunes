﻿using Microsoft.AspNetCore.Identity;
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
        private readonly IDistributedCache _cache;

        private readonly UserManager<User> _userManager;

        private readonly ApplicationContext _context;

        public QueueHub(IDistributedCache cache, UserManager<User> manager, ApplicationContext context)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _userManager = manager ?? throw new ArgumentNullException(nameof(manager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task JoinQueue(string queueName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, queueName).ConfigureAwait(false);
        }

        public async Task LeaveQueue(string queueName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, queueName).ConfigureAwait(false);
        }

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

            Console.WriteLine(JsonConvert.SerializeObject(q));

            List<Song> list = BuildSongList(q);

            list.RemoveAll(song => song.Id == spotifyId);
            
            Console.WriteLine(JsonConvert.SerializeObject(list));

            q = BuildSongQueue(list);

            await _cache.SetStringAsync($"$queue:${partyId}", JsonConvert.SerializeObject(q)).ConfigureAwait(false);

            await Clients.All.SendAsync("ReceiveRemoveSong", spotifyId).ConfigureAwait(false);
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
