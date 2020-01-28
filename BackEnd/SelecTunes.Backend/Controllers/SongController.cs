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
                return new BadRequestObjectResult("Object is null");
            }

            using HttpClient c = _cf.CreateClient("spotify");
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=track&q={0}", HttpUtility.UrlEncode(songToSearch.QueryString)));
            r.Headers.Add("Authorization", "Bearer BQDUDCfofD74StCVenxbz10pLr4lxt6ipDp1QNQmd8phbPsDs1GX9SB646djq8jRlRu5oPbQhT7gZH3ZFzGHPwc2Wv26B3duKsgfN_ZPgnmR0mroYMkv9Yh2v0tUVENKcGswg7N-w9_-62Q141R_");
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

            //Console.WriteLine(await s.Content.ReadAsStringAsync().ConfigureAwait(false));

            var SongReponses = JsonConvert.DeserializeObject<SpotifyTracksResponseBody>(s.ToString());

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
            using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, string.Format("search?limit=10&market=US&type=artist&q={0}", HttpUtility.UrlEncode(artistToSearch.QueryString)));
            r.Headers.Add("Authorization", "Bearer BQDUDCfofD74StCVenxbz10pLr4lxt6ipDp1QNQmd8phbPsDs1GX9SB646djq8jRlRu5oPbQhT7gZH3ZFzGHPwc2Wv26B3duKsgfN_ZPgnmR0mroYMkv9Yh2v0tUVENKcGswg7N-w9_-62Q141R_");
            HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

            //Console.WriteLine(await s.Content.ReadAsStringAsync().ConfigureAwait(false));

            var SongReponses = JsonConvert.DeserializeObject<SpotifyTracksResponseBody>(s.ToString());

            return Ok(SongReponses);
        }
    }
}
