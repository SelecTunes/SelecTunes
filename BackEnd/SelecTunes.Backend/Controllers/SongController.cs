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

        public SongController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, ILogger<SongController> logger, UserManager<User> userManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _cf = factory ?? throw new ArgumentNullException(nameof(factory));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> SearchBySong([FromBody]SearchQuery songToSearch)
        {
            /*if (songToSearch == null)
            {
                _logger.LogDebug("DEBUG");
                _logger.LogDebug(String.Format("SEARCH OBJECT IS NULL: {}"), songToSearch);
                return new BadRequestObjectResult(songToSearch);
            }

            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // get the current user identity
            Party party = _context.Parties.Where(p => p.PartyMembers.Any(a => a.Id == user.Id) || p.PartyHost == user).FirstOrDefault(); // find which party they are a member of

            if (party == null)
            {
                _logger.LogWarning("GTFO User. You ain't in no party.");

                throw new InvalidOperationException("No search without party");
            }

            string bearerCode = party.PartyHost.SpotifyAccessToken; // get the access token of that party's host

            using HttpClient c = _cf.CreateClient("spotify");
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=track&q={0}", HttpUtility.UrlEncode(songToSearch.QueryString)));
            r.Headers.Add("Authorization", String.Format("Bearer {0}", bearerCode));
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);
            if (s.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("WARN");
                _logger.LogError("403 UNAUTHORIZED");
                return Unauthorized("Please try again");
            }

            var ToParse = s.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var SongReponses = JsonConvert.DeserializeObject<SpotifyTracksResponseBody>(ToParse, settings);

            return Ok(SongReponses);*/

            try
            {
                SpotifyTracksResponseBody artists = await Search<SpotifyTracksResponseBody>("track", songToSearch, HttpContext.User).ConfigureAwait(false);

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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<String>> SearchByArtist([FromBody]SearchQuery artistToSearch)
        {
            /*if (artistToSearch == null)
            {
                _logger.LogDebug("DEBUG");
                _logger.LogDebug(String.Format("SEARCH OBJECT IS NULL: {}"), artistToSearch);
                return new BadRequestObjectResult("Object is null");
            }

            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false); // get the current user identity
            Party party = _context.Parties.Where(p => p.PartyMembers.Any(a => a.Id == user.Id) || p.PartyHost == user).FirstOrDefault(); // find which party they are a member of

            if (party == null)
            {
                _logger.LogWarning("GTFO User. You ain't in no party.");

                throw new InvalidOperationException("No search without party");
            }

            string bearerCode = party.PartyHost.SpotifyAccessToken; // get the access token of that party's host

            using HttpClient c = _cf.CreateClient("spotify");
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=artist&q={0}", HttpUtility.UrlEncode(artistToSearch.QueryString)));
            r.Headers.Add("Authorization", String.Format("Bearer {0}", bearerCode));
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);
            if (s.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogError("WARN");
                _logger.LogError("403 UNAUTHORIZED");
                return Unauthorized("Please try again");
            }

            var ToParse = s.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };*/

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
         * Func Search<T>() -> async <T>
         * => SpotifyResult
         * 
         * This method wraps the spotify api and search for songs and artists
         * 
         * 16/02/2020 D/M/Y - Alexander Young - Finalize
         */
        private async Task<T> Search<T>(string type, SearchQuery query, ClaimsPrincipal context)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            User user = await _userManager.GetUserAsync(context).ConfigureAwait(false); // get the current user identity
            Party party = _context.Parties.Where(p => p.PartyMembers.Any(a => a.Id == user.Id) || p.PartyHost == user).FirstOrDefault(); // find which party they are at

            if (party == null)
            {
                _logger.LogWarning("Attempted Song search without being at a party.");

                throw new InvalidOperationException("No search without party");
            }

            string bearerCode = party.PartyHost.SpotifyAccessToken; // get the access token of that party's host

            using HttpClient c = _cf.CreateClient("spotify");

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                bearerCode
            );
            //using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=artist&q={0}", HttpUtility.UrlEncode(artistToSearch.QueryString)));

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
                _logger.LogError("403 UNAUTHORIZED RETURNED FROM SPOTIFY", response);
                throw new UnauthorizedAccessException();
            }

            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
