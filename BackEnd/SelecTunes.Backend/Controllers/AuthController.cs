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
using System.Net;
using SelecTunes.Backend.Helper;
using Microsoft.Extensions.Options;

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

        private readonly IOptions<AppSettings> _options;

        private readonly AuthHelper _auth;

        public AuthController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, IConfiguration config, IOptions<AppSettings> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _context = context;
            _cache = cache;
            _cf = factory;
            _config = config;
            _options = options;

            _auth = new AuthHelper()
            {
                ClientFactory = _cf,
                ClientSecret = _options.Value.ClientSecret,
                ClientId = _options.Value.ClientId,
                RedirectUrl = _options.Value.RedirectUri,
            };

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

            string clientHeader = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    $"{_config.GetConnectionString("Spotify_ClientId")}:{_config.GetConnectionString("Spotify_ClientSecret")}"
                )
            );

            using (HttpClient c = _cf.CreateClient("spotify-accounts")) // create a new client to spotify, see Startup
            {
                // Add the Authorization Header, now that it exists.
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    clientHeader
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
                    Console.WriteLine("Got Response {0}", s);

                    AccessAuthToken content = JsonConvert.DeserializeObject<AccessAuthToken>(s);
                    Console.WriteLine(content);
                    int attempt = 0;

                    RetryAuth:
                    using (HttpClient x = _cf.CreateClient("spotify"))
                    {
                        Console.WriteLine("Trying with Access {0}", content.AccessToken);
                        x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                            "Bearer",
                            content.AccessToken
                        );

                        HttpResponseMessage w = await x.GetAsync("me").ConfigureAwait(false);

                        Console.WriteLine(w.StatusCode);

                        String b = await w.Content.ReadAsStringAsync().ConfigureAwait(false);

                        Console.WriteLine("Response Message {0}", b);

                        if (w.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            using (FormUrlEncodedContent fc = new FormUrlEncodedContent(new[]
                            {
                                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                                new KeyValuePair<string, string>("refresh_token", content.RefreshToken)
                            }))
                            {
                                HttpResponseMessage v = await c.PostAsync("token", fc).ConfigureAwait(false);

                                String t = await r.Content.ReadAsStringAsync().ConfigureAwait(false);
                                AccessAuthToken tok = JsonConvert.DeserializeObject<AccessAuthToken>(t);
                                Console.WriteLine("Retrying Auth");
                                Console.WriteLine(tok.AccessToken);
                                Console.WriteLine(tok.ExpiresIn);
                                Console.WriteLine(tok.Scope);
                                Console.WriteLine(tok.TokenType);
                                Console.WriteLine(content.RefreshToken);
                                Console.WriteLine(attempt++);

                                content.AccessToken = tok.AccessToken;
                                content.ExpiresIn = tok.ExpiresIn;
                                content.Scope = tok.Scope;
                                content.TokenType = tok.TokenType;

                                goto RetryAuth;
                            }
                        }

                        SpotifyIdentity identity = JsonConvert.DeserializeObject<SpotifyIdentity>(b);
                        Console.WriteLine(b);

                        return Ok(identity.DisplayName);
                    }
                }
            }
        }
    }
}
