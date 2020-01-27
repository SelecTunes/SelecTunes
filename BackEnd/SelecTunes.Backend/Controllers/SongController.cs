using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models.SongSearchIngestion;
using SpotifyAPI.Web;


namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
        public ActionResult<String> SearchBySong([FromBody]SearchQuery songToSearch)
        {
            if (songToSearch == null)
            {
                return new BadRequestObjectResult("Object is null");
            }
            return Ok("Index");
        }

        [HttpPost]
        public ActionResult<String> SearchByAlbum([FromBody]SearchQuery albumToSearch)
        {
            if (albumToSearch == null)
            {
                return new BadRequestObjectResult("Object is null");
            }
            return Ok("Index");
        }

        [HttpPost]
        public ActionResult<String> SearchByArtist([FromBody]SearchQuery artistToSearch)
        {
            if (artistToSearch == null)
            {
                return new BadRequestObjectResult("Object is null");
            }
            return Ok("Index");
        }
    }
}
