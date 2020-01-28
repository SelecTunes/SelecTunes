using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;
using SelecTunes.Backend.Models.Auth;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        private readonly IHttpClientFactory _cf;

        private readonly IConfiguration _config;

        public AuthController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, IConfiguration config)
        {
            _context = context;
            _cache = cache;
            _cf = factory;
            _config = config;
        }

        [HttpPost]
        public ActionResult<String> Login([FromBody]HostUser newHost)
        {
            if (newHost == null)
            {
                return new BadRequestObjectResult("Object is null");
            }

            Console.WriteLine(JsonConvert.SerializeObject(newHost));

            // String serial = JsonConvert.SerializeObject(p); // Serialize the redis entries, as we only have one cache

            // _cache.SetString($"$party:${p.Id}", serial, new DistributedCacheEntryOptions()); // Default never expire //TO TEST LATER

            return Ok("Index");
        }
        
        [HttpGet]
        public async Task<ActionResult<String>> CallbackAsync([FromQuery]SpotifyLogin login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            using (HttpClient c = _cf.CreateClient("spotify-accounts")) // create a new client to spotify, see Startup
            {
                // Add the Authorization Header, now that it exists.
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $"{_config.GetConnectionString("Spotify_ClientId")}:{_config.GetConnectionString("Spotify_ClientSecret")}"
                        )
                    )
                );

                using (FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("redirect_uri", "https://localhost:44395/api/auth/callback"),
                    new KeyValuePair<string, string>("code", login.Code)
                }))
                {
                    HttpResponseMessage r = await c.PostAsync("token", formContent).ConfigureAwait(false);

                    String s = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                    dynamic content = JsonConvert.DeserializeObject<dynamic>(s);
                    Console.WriteLine(content);


                    return Ok(s);
                }
            }
        }
    }
}
