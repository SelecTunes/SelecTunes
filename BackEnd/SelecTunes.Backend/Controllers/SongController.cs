using System;
using System.Web;
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
            if (songToSearch == null)
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
           
            return Ok(SongReponses);
        }

        [HttpGet]
        public async Task<ActionResult<String>> SearchByArtist([FromBody]SearchQuery artistToSearch)
        {
            _logger.LogDebug("DEBUG");
            _logger.LogDebug(String.Format("Querying artist with query: {}", artistToSearch.QueryString));
            if (artistToSearch == null)
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
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=track&q={0}", HttpUtility.UrlEncode(songToSearch.QueryString)));
            r.Headers.Add("Authorization", String.Format("Bearer {0}", bearerCode));

            if (s.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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

            return Ok(SongReponses);
        }
    }
}
