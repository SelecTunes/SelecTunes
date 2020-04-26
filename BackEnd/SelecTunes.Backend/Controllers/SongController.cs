using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Helper;
using SelecTunes.Backend.Models;
using SelecTunes.Backend.Models.OneOff;
using SelecTunes.Backend.Models.SongSearchIngestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class SongController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        private readonly IHttpClientFactory _cf;

        private readonly ILogger<SongController> _logger;

        private readonly UserManager<User> _userManager;

        private readonly AuthHelper _auth;

        public SongController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, ILogger<SongController> logger, UserManager<User> userManager, AuthHelper auth)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cf = factory ?? throw new ArgumentNullException(nameof(factory));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _auth = auth ?? throw new ArgumentNullException(nameof(auth));
        }

        /**
         * Func SearchByArtist(<SearchQuery> :songToSearch) -> async <ActionResult<SpotifyTracksResponseBody>>
         * => SpotifyArtistResponseBody
         * 
         * GET request to /api/song/searchbysong with a JSON body of {"queryString":"name"}
         * 
         * 15/02/2020 D/M/Y - Nathan Tucker - Creation
         */
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SpotifyTracksResponseBody>> SearchBySong([FromBody]SearchQuery songToSearch)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // Find the current user asking to join a party
            Party party = _context.Parties.Where(p => p == user.Party || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of

            try
            {
                SpotifyTracksResponseBody tracks = await Search<SpotifyTracksResponseBody>("track", songToSearch, user, party).ConfigureAwait(false);
                if (!party.AllowExplicit)
                {
                    tracks.Tracks.Items = tracks.Tracks.Items.Where(x => !x.Explicit).ToList();

                    return Ok(tracks);
                }
                return Ok(tracks);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgNullException Occurred: {0}", e);
                _logger.LogDebug("Search Called with NULL Query {}", e);
                return BadRequest("Query is null.");
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException Occurred: {0}", e);
                _logger.LogDebug("Search Attempted without Being at Party First", e);

                return Unauthorized("You must be at a party to search for songs.");
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("UnauthorizedAccessException Occurred: {0}", e);
                _logger.LogWarning("Spotify returned a UnauthorizedAccessException", e);

                return Unauthorized("Party Host not authorized to preform that action.");
            }
        }

        /**
         * Func SearchByArtist(<SearchQuery> :artistToSearch) -> async <ActionResult<SpotifyArtistResponseBody>>
         * => SpotifyArtistResponseBody
         * 
         * GET request to /api/song/searchbyartist with a JSON body of {"queryString":"name"}
         * 
         * 15/02/2020 D/M/Y - Nathan Tucker - Creation
         */
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<SpotifyArtistResponseBody>> SearchByArtist([FromBody]SearchQuery artistToSearch)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // Find the current user asking to join a party
            Party party = _context.Parties.Where(p => p == user.Party || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of

            try
            {
                SpotifyArtistResponseBody artists = await Search<SpotifyArtistResponseBody>("artist", artistToSearch, user, party).ConfigureAwait(false);
                
                return Ok(artists);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogDebug("Search Called with NULL Query {}", e);
                return BadRequest("Query is null.");
            }
            catch (InvalidOperationException e)
            {
                _logger.LogDebug("Search Attempted without Being at Party First", e);

                return Unauthorized("You must be at a party to search for songs.");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogWarning("Spotify returned a UnauthorizedAccessException", e);

                return Unauthorized("Party Host not authorized to preform that action.");
            }
        }

        /**
         * Func AddToQueue(<Song> :SongToAdd) -> async <ActionResult<String>>
         * => true
         *
         * 
         * Send a POST request to /api/song/addtoqueue with the unique spotify song id
         * Find the current user, and then the party tied to the user
         * Add to that party's cache
         *
         * 21/02/2020 - Nathan Tucker
         */
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<String>> AddToQueue([FromBody]Song SongToAdd)
        {
            if (SongToAdd == null)
            {
                return BadRequest("Query is null.");
            }

            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // Find the current user asking to join a party
            Party party = _context.Parties.Where(p => p == user.Party || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of

            if (party == null)
            {
                throw new InvalidOperationException("Yo party is nul");
            }

            if (user == null)
            {
                throw new InvalidOperationException("User is null");
            }

            var ByteQueue = await _cache.GetAsync($"$queue:${party.JoinCode}").ConfigureAwait(false);
            if (ByteQueue == null)
            {
                Queue<Song> queue = new Queue<Song>();
               
                queue.Enqueue(SongToAdd);
                await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(queue)).ConfigureAwait(false);
                
                return Ok(new { Success = true });
            }

            byte[] q = await _cache.GetAsync($"$locked:${party.JoinCode}").ConfigureAwait(false);
            if (q == null)
            {
                await _cache.SetStringAsync($"$locked:${party.JoinCode}", JsonConvert.SerializeObject(new Queue<Song>())).ConfigureAwait(false);
            }

            Queue<Song> CurrentQueue = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(ByteQueue));

            if (CurrentQueue == null)
            {
                Queue<Song> queue = new Queue<Song>();
                queue.Enqueue(SongToAdd);
                await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(queue)).ConfigureAwait(false);
                return Ok(new { Success = true });
            }

            CurrentQueue.Enqueue(SongToAdd);

            // Write the new queue to the redis cache. Because it used the old key from the key value pair, the old one will be written over
            await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(CurrentQueue)).ConfigureAwait(false);

            return Ok(new { Success = true });
        }

        /**
         * Func Queue() -> async <ActionResult<String>>
         * => JSON representation of Song Queue of the current User's Party
         *
         * Send a GET request to the endpoint to get a queue
         * Find the current user from the context
         * Find the party that user is a member of
         * Pull the queue out of the redis cache
         * Return the string representation
         *
         * 06/03/2020 - Nathan Tucker 
         */
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<String>> Queue()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // Find the current user asking to join a party

            if (user == null)
            {
                return Unauthorized("User needs to log in");
            }

            Party party = _context.Parties.Where(p => p == user.Party || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of

            if (party == null)
            {
                return NotFound("No party exists for this user");
            }

            byte[] ByteQueue = await _cache.GetAsync($"$queue:${party.JoinCode}").ConfigureAwait(false);
            if (ByteQueue == null)
            {
                return BadRequest(new { Success = false });
            }

            byte[] locked = await _cache.GetAsync($"$locked:${party.JoinCode}").ConfigureAwait(false);
            if (locked == null)
            {
                return BadRequest(new { Success = false });
            }

            Queue<Song> lockedIn = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(locked));

            Queue<Song> queue = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(ByteQueue));

            return Ok(new { LockedIn = lockedIn, Votable = queue});
        }

        public partial class GetDeleteThis
        {
            public string Id { get; set; }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<string>> ThisSongWasPlayed([FromBody]GetDeleteThis id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            User user = await _userManager.GetUserAsync(User).ConfigureAwait(false);

            if (user == null)
            {
                return BadRequest("You bad user.");
            }

            Party party = _context.Parties.Where(p => p == user.Party || p.PartyHostId == user.Id || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of
        
            if (party == null)
            {
                return BadRequest("You ded");
            }

            if (party.PartyHost != user || party.PartyHostId != user.Id)
            {
                return BadRequest("You not host. Please go away");
            }

            byte[] locked = await _cache.GetAsync($"$locked:${party.JoinCode}").ConfigureAwait(false);
            if (locked == null)
            {
                return BadRequest(new { Success = false, Error = "Party hath no locked" });
            }

            Queue<Song> lockedIn = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(locked));

            if (lockedIn.Peek().Id == id.Id)
            {
                lockedIn.Dequeue();
            }

            await _cache.SetStringAsync($"$locked:${party.JoinCode}", JsonConvert.SerializeObject(lockedIn)).ConfigureAwait(false);

            return Ok(new { Success = true });
        }

        /**
         * Func Search<T>() -> async <T>
         * => SpotifyResult
         * 
         * This method wraps the spotify api and search for songs and artists
         *
         * 15/02/2020 D/M/Y - Nathan Tucker - Creation
         * 16/02/2020 D/M/Y - Alexander Young - Finalize
         */
        private async Task<T> Search<T>(string type, SearchQuery query, User user, Party party)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (party == null)
            {
                _logger.LogWarning("Attempted Song search without being at a party.");

                throw new InvalidOperationException("No search without party");
            }

            Console.WriteLine("User Party Id: {0}. Party Id: {1}, Party Host: {2}", user.Id, party.Id, party.PartyHost);

            if (party.PartyHost == null)
            {
                User hostUser = _context.Users.Where(u => u.Id == party.PartyHostId).FirstOrDefault();
                party.PartyHost = hostUser ?? throw new InvalidOperationException($"Party is without party host. Cannot find user with ID {party.PartyHostId}");
                _context.SaveChanges();
            }

            User host = party.PartyHost = await _auth.UpdateUserTokens(party.PartyHost).ConfigureAwait(false);
            _context.SaveChanges();

            string bearerCode = host.Token.AccessToken; // get the access token of that party's host

            using HttpClient c = _cf.CreateClient("spotify");

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                bearerCode
            );

            HttpResponseMessage s = await c.GetAsync(QueryHelpers.AddQueryString("search", new Dictionary<string, string>
            {
                ["limit"] = "10",
                ["market"] = "US",
                ["type"] = type,
                ["q"] = query.QueryString,
            })).ConfigureAwait(false);

            string response = await s.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (s.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("401 Unauthorized was returned from Spotify {}", response);
                throw new UnauthorizedAccessException();
            }

            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
