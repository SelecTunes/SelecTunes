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
using Microsoft.Extensions.Configuration;
using SelecTunes.Backend.Helper;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Logging;

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

        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory, IConfiguration config, IOptions<AppSettings> options, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AuthController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Throw nil arg expection if context is nil.
            _cache = cache ?? throw new ArgumentNullException(nameof(cache)); // Throw nil arg expection if cache is nil.
            _cf = factory ?? throw new ArgumentNullException(nameof(factory)); // Throw nil arg expection if factory is nil.
            _config = config ?? throw new ArgumentNullException(nameof(config)); // Throw nil arg expection if config is nil.
            _options = options ?? throw new ArgumentNullException(nameof(options)); // Throw nil arg expection if options is nil.
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager)); // "
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager)); // "
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // "

            _auth = new AuthHelper()
            { // Initialize the Auth Helper.
                ClientFactory = _cf,
                ClientSecret = _options.Value.ClientSecret,
                ClientId = _options.Value.ClientId,
                RedirectUrl = _options.Value.RedirectUri,
            };
        }

        // Example Code to get the Currently Logged In User.
        [HttpGet]
        public async Task<ActionResult<String>> GetCurrentUserEmail()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);

            Console.WriteLine(user);

            return user.Email;
        }

        /**
         * Func Register(<InputModel> :model) -> async <ActionResult<String>>
         * => SignInResult.Succeeded
         * 
         * 1. Takes a username and password.
         * 2. Attempt Register with Identity.
         * 3. Return Result.
         * 
         * 11/02/2020 D/M/Y - Alexander Young - Finalize
         */
        [HttpPost]
        public async Task<ActionResult<String>> Register([FromForm]InputModel model)
        {
            if (model == null)
            {
                _logger.LogError("INPUT MODEL IS NULL");
                throw new ArgumentNullException(nameof(model));
            }

            _logger.LogDebug("DEBUG");
            _logger.LogDebug("REGISTERING USER: {}", model.Email);

            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, EmailConfirmed = true, UserName = model.Email };
                IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);

                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true).ConfigureAwait(false);
                    return new JsonResult(true);
                }

                return new JsonResult(identityResult.Errors);
            }

            return new JsonResult(ModelState);
        }

        /**
         * Func Login(<InputModel> :model) -> async <ActionResult<String>>
         * => SignInResult.Succeeded
         * 
         * 1. Takes a username and password.
         * 2. Attempt Login with Identity.
         * 3. Return Result.
         * 
         * 11/02/2020 D/M/Y - Alexander Young - Finalize
         */
        [HttpPost]
        public async Task<ActionResult<String>> Login([FromForm]InputModel model)
        {
            if (model == null)
            {
                _logger.LogError("LOGIN MODEL IS NULL");
                throw new ArgumentNullException(nameof(model));
            }

            if (ModelState.IsValid)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, lockoutOnFailure: false).ConfigureAwait(false);
                
                if (result.Succeeded)
                {
                   return new JsonResult(new { Success = true });
                }

                if (result.IsNotAllowed)
                {
                    return new JsonResult(new { Success = false, Error = "Account Is Not Allowed Login" });
                }

                if (result.IsLockedOut)
                {
                    return new JsonResult(new { Success = false, Error = "Account Is Locked Out" });
                }

                return new JsonResult(new { Success = false, Error = "Login Failure" });
            }

            return new JsonResult(ModelState);
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
        [Authorize]
        public async Task<ActionResult<String>> Callback([FromQuery]SpotifyLogin login)
        {
            if (login == null)
            { // If login is nil, throw a nil arg expection.
                throw new ArgumentNullException(nameof(login));
            }

            AccessAuthToken tok = await _auth.TransmutAuthCode(login.Code).ConfigureAwait(false); // Change that login code to an access token and refresh token.

            Console.WriteLine(tok);

            tok = await _auth.AssertValidLogin(tok, false).ConfigureAwait(false); // Make sure its valid.

            using HttpClient c = _cf.CreateClient("spotify"); // Create a new client to spotify.

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                tok.AccessToken
            ); // Now you are thinking with Bearers.

            HttpResponseMessage me = await c.GetAsync(new Uri("me", UriKind.Relative)).ConfigureAwait(false); // Get the Auth Users information.

            string response = await me.Content.ReadAsStringAsync().ConfigureAwait(false); // Make it a string.

            SpotifyIdentity identify = JsonConvert.DeserializeObject<SpotifyIdentity>(response); // Make it a SpotifyIdentity.

            System.IO.File.WriteAllText("responses.txt", response);

            User host = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);

            if (host == null)
            { // If not, reject
                return new BadRequestObjectResult(new { Success = false, Error = "User has not yet registered with the Identity Provider." });
            }
            
            // If so, update the tokens.
            host.SpotifyAccessToken = tok.AccessToken;
            host.SpotifyRefreshToken = tok.RefreshToken;

            Party party = new Party
            { // Create a new party with this user as a host.
                JoinCode = _rand.Next(0, 100000).ToString(CultureInfo.InvariantCulture).PadLeft(6, '0'),
                PartyHost = host,
            };

            _context.Parties.Add(party);

            _context.SaveChanges(); // Kommit to DB.

            return new JsonResult(new { JoinCode = party.JoinCode }); // Return Party Join Code.
        }
    }
}
