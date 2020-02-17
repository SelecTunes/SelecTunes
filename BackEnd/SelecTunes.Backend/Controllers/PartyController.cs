using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;
using System.Linq;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        private readonly UserManager<User> _userManager;

        private readonly ILogger<PartyController> _logger;

        public PartyController(ApplicationContext context, IDistributedCache cache, UserManager<User> userManager, ILogger<PartyController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /**
         * Func JoinParty(string :joinCode)
         * => ActionResult<String>
         * 
         * Send a POST request to /api/party/joinparty with a join code
         * The user that sent the request will be added to the party with that join code
         *
         * 15/02/2020 D/M/Y - Nathan Tucker - Stubbing
         */
        [HttpPost]
        [Authorize]
        public ActionResult<String> JoinParty([FromBody]string joinCode)
        {
            Party party = _context.Parties.Where(p => p.JoinCode == joinCode).FirstOrDefault();
            if (party == null)
            {
                throw new InvalidOperationException("a party with that code does not exist");
            }
            party.PartyMembers.Add(_userManager.GetUserAsync(HttpContext.User).Result);

            return null;
        }

        /**
         * Func LeaveParty()
         * => ActionResult<String>
         * 
         * Send a POST request to /api/party/leaveparty
         * The user that sent the request will be removed from the party which they are currently in
         *
         * 15/02/2020 D/M/Y - Nathan Tucker - Stubbing
         */
        [HttpPost]
        [Authorize]
        public ActionResult<String> LeaveParty()
        {
            throw new NotImplementedException("This code has not yet been implemented");
        }
    }
}
