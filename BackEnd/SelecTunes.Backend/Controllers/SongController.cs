using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models.SongSearchIngestion;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using SelecTunes.Backend.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using SelecTunes.Backend.Helper;
using System.Text;

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
            try
            {
                SpotifyTracksResponseBody artists = await Search<SpotifyTracksResponseBody>("track", songToSearch, HttpContext.User).ConfigureAwait(false);

                return Ok(artists);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgNullException Occurred: {0}", e);
                _logger.LogDebug("Search Called with NULL Query {}", e);
                return new BadRequestObjectResult("Query is null.");
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
            try
            {
                SpotifyArtistResponseBody artists = await Search<SpotifyArtistResponseBody>("artist", artistToSearch, HttpContext.User).ConfigureAwait(false);
                
                return Ok(artists);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogDebug("Search Called with NULL Query {}", e);
                return new BadRequestObjectResult("Query is null.");
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
                return new BadRequestObjectResult("Query is null.");
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
            //Console.WriteLine("THIS IS WHAT THE QUEUE LOOKS LIKE: {0}", Encoding.UTF8.GetString(ByteQueue));
            //Console.WriteLine("BYTE: {0}", ByteQueue.ToString());
            //Console.WriteLine("BYTE SERIALIZE: {0}", JsonConvert.SerializeObject(ByteQueue));
            if (ByteQueue == null)
            {
                Console.WriteLine("CREATING NEW QUEUE. BYTE QUEUE IS NULL");
                Queue<Song> queue = new Queue<Song>();
                queue.Enqueue(SongToAdd);
                await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(queue)).ConfigureAwait(false);
                return new JsonResult(new { Success = true });
            }

            Queue<Song> CurrentQueue = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(ByteQueue));

            //Console.WriteLine("THIS IS WHAT THE DESERIALIZED QUEUE LOOKS LIKE: {0}", CurrentQueue);
            //Console.WriteLine("CURRENT: {0}", CurrentQueue.ToString());
            //Console.WriteLine("CURRENT SERIALIZE: {0}", JsonConvert.SerializeObject(CurrentQueue));
            if (CurrentQueue == null)
            {
                Console.WriteLine("CREATING NEW QUEUE. CURRENT QUEUE IS NULL");
                Queue<Song> queue = new Queue<Song>();
                queue.Enqueue(SongToAdd);
                await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(queue)).ConfigureAwait(false);
                return new JsonResult(new { Success = true });
            }

            CurrentQueue.Enqueue(SongToAdd);

            Console.WriteLine("WRITING NEW QUEUE, {0}", JsonConvert.SerializeObject(CurrentQueue));

            // Write the new queue to the redis cache. Because it used the old key from the key value pair, the old one will be written over
            await _cache.SetStringAsync($"$queue:${party.JoinCode}", JsonConvert.SerializeObject(CurrentQueue)).ConfigureAwait(false);

            return new JsonResult(new { Success = true });
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
            Party party = _context.Parties.Where(p => p == user.Party || p.Id == user.PartyId).FirstOrDefault(); // find the party that they are member of

            if (user == null)
            {
                throw new InvalidOperationException("User is null");
            }

            if (party == null)
            {
                throw new InvalidOperationException("User is not in a party");
            }

            var ByteQueue = await _cache.GetAsync($"$queue:${party.JoinCode}").ConfigureAwait(false);
            if (ByteQueue == null)
            {
                return new JsonResult(new { Success = false });
            }

            Queue<Song> queue = JsonConvert.DeserializeObject<Queue<Song>>(Encoding.UTF8.GetString(ByteQueue));

            return Ok(queue);
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
        private async Task<T> Search<T>(string type, SearchQuery query, ClaimsPrincipal context)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            User user = await _userManager.GetUserAsync(context).ConfigureAwait(false); // get the current user identity
            Party party = _context.Parties.Where(p => p.Id == user.PartyId || p.PartyHost == user || p.PartyHostId == user.Id).FirstOrDefault(); // find which party they are at

            if (party == null)
            {
                _logger.LogWarning("Attempted Song search without being at a party.");

                throw new InvalidOperationException("No search without party");
            }

            Console.WriteLine("User Party Id: {0}. Party Id: {1}, Party Host: {2}", user.Id, party.Id, party.PartyHost);

            if (party.PartyHost == null)
            {
                User hostUser = _context.Users.Where(u => u.Id == party.PartyHostId).FirstOrDefault();

                if (hostUser == null)
                {
                    throw new InvalidOperationException($"Party is without party host. Cannot find user with ID {party.PartyHostId}");
                }

                party.PartyHost = hostUser;
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
