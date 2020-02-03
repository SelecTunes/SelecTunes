using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models.SongSearchIngestion;
using SpotifyAPI.Web;
using Newtonsoft.Json;


namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class SongController : ControllerBase
    {

        private static SpotifyWebAPI _spotify;

        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        private readonly IHttpClientFactory _cf;

        public SongController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory)
        {
            _context = context;
            _cache = cache;
            _cf = factory;
            _spotify = new SpotifyWebAPI()
            {
                AccessToken = "REEEEEE",
                TokenType = "Bearer"
            };
        }

        [HttpPost]
        public async Task<ActionResult> SearchBySong([FromBody]SearchQuery songToSearch)
        {
            if (songToSearch == null)
            {
                return new BadRequestObjectResult(songToSearch);
            }

            using HttpClient c = _cf.CreateClient("spotify");
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=track&q={0}", HttpUtility.UrlEncode(songToSearch.QueryString)));
            r.Headers.Add("Authorization", "Bearer BQCMlM1KBu-NaSQSoLBd1GwfrskKB521uVaCZqYnnkOK-EN2hSLRrIg3EgOFzpG_a4HmmYr4vN4A7yoMSpbZDEsLjMGBztphZ5m136DQOPx02nA1mnJ_xWvmPWZWOGltkQX72-DDkWPvJwd-WoEk");
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

            var ToParse = s.Content.ReadAsStringAsync().Result;
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var SongReponses = JsonConvert.DeserializeObject<SpotifyTracksResponseBody>(ToParse, settings);

            return Ok(SongReponses);
        }

        [HttpPost]
        public async Task<ActionResult<String>> SearchByArtist([FromBody]SearchQuery artistToSearch)
        {
            if (artistToSearch == null)
            {
                return new BadRequestObjectResult("Object is null");
            }

            using HttpClient c = _cf.CreateClient("spotify");
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=track&q={0}", HttpUtility.UrlEncode(artistToSearch.QueryString)));
            r.Headers.Add("Authorization", "Bearer BQCMlM1KBu-NaSQSoLBd1GwfrskKB521uVaCZqYnnkOK-EN2hSLRrIg3EgOFzpG_a4HmmYr4vN4A7yoMSpbZDEsLjMGBztphZ5m136DQOPx02nA1mnJ_xWvmPWZWOGltkQX72-DDkWPvJwd-WoEk");
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

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
