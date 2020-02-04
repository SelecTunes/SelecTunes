﻿using System;
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
using System.Linq;
using System.Globalization;

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

        private readonly Random _rand = new Random();

        public AuthController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, IConfiguration config, IOptions<AppSettings> options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Throw nil arg expection if context is nil.
            _cache = cache ?? throw new ArgumentNullException(nameof(cache)); // Throw nil arg expection if cache is nil.
            _cf = factory ?? throw new ArgumentNullException(nameof(factory)); // Throw nil arg expection if factory is nil.
            _config = config ?? throw new ArgumentNullException(nameof(config)); // Throw nil arg expection if config is nil.
            _options = options ?? throw new ArgumentNullException(nameof(options)); // Throw nil arg expection if options is nil.

            _auth = new AuthHelper()
            { // Initialize the Auth Helper.
                ClientFactory = _cf,
                ClientSecret = _options.Value.ClientSecret,
                ClientId = _options.Value.ClientId,
                RedirectUrl = _options.Value.RedirectUri,
            };

        }

        // Lol Example Code.
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
        

        /**
         * Func Callback(<SpotifyLogin> :login) -> async <ActionResult<String>>
         * => Party.JoinCode
         * 
         * 1. Takes in a Spotify login token, and changes it into a accesstoken.
         * 2. Then looks if there is this user already in the db. If not, create.
         * 3. Creates a new party, with a NOT CRYPTOGRAPHICALLY SECURE join code.
         * 
         * 03/02/2020 D/M/Y - Alexander Young - Finalize
         */
        [HttpGet]
        public async Task<ActionResult<String>> Callback([FromQuery]SpotifyLogin login)
        {
            if (login == null)
            { // If login is nil, throw a nil arg expection.
                throw new ArgumentNullException(nameof(login));
            }

            AccessAuthToken tok = await _auth.TransmutAuthCode(login.Code).ConfigureAwait(false); // Change that login code to an access token and refresh token.

            Console.WriteLine(tok);

            tok = await _auth.AssertValidLogin(tok).ConfigureAwait(false); // Make sure its valid.

            using HttpClient c = _cf.CreateClient("spotify"); // Create a new client to spotify.

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tok.AccessToken
            ); // Now you are thinking with Bearers.

            HttpResponseMessage me = await c.GetAsync(new Uri("me", UriKind.Relative)).ConfigureAwait(false); // Get the Auth Users information.

            string response = await me.Content.ReadAsStringAsync().ConfigureAwait(false); // Make it a string.

            SpotifyIdentity identify = JsonConvert.DeserializeObject<SpotifyIdentity>(response); // Make it a SpotifyIdentity.

            HostUser host = _context.HostUsers.Where(x => x.Email == identify.Email).FirstOrDefault(); // See if the user exists already.

            if (host == null)
            { // If not, make 'em.
                host = new HostUser
                {
                    Email = identify.Email,
                    SpotifyAccessToken = tok.AccessToken,
                    SpotifyRefreshToken = tok.RefreshToken,
                    IsBanned = false,
                    PhoneNumber = "515-555-1234",
                    UserName = identify.DisplayName,
                };

                _context.HostUsers.Add(host);
            }
            else
            { // If so, update the tokens.
                host.SpotifyAccessToken = tok.AccessToken;
                host.SpotifyRefreshToken = tok.RefreshToken;
            }

            Party party = new Party
            { // Create a new party with this user as a host.
                JoinCode = _rand.Next(0, 100000).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'),
                PartyHost = host,
            };

            _context.Parties.Add(party);

            _context.SaveChanges(); // Kommit to DB.

            return Ok(party.JoinCode); // Return Party Join Code.
        }
    }
}
