using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        private readonly UserManager<User> _userManager;

        private readonly ILogger<PartyController> _logger;

        public PartyController(ApplicationContext context, UserManager<User> userManager, ILogger<PartyController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /**
         * Func JoinParty(string :joinCode) -> ActionResult<String>
         * => party join code
         * 
         * Send a POST request to /api/party/joinparty with a join code
         * The user that sent the request will be added to the party with that join code
         *
         * 15/02/2020 D/M/Y - Nathan Tucker - Stubbing
         * 17/02/2020 D/M/Y - Nathan Tucker - Finalizing
         */
        [HttpPost]
        [Authorize]
        public ActionResult<String> JoinParty([FromForm]string joinCode)
        {
            _logger.LogDebug("User {0} attempting to join party with join code {1}", _userManager.GetUserAsync(HttpContext.User).Result, joinCode);

            User ToJoin = _userManager.GetUserAsync(HttpContext.User).Result; // Find the current user asking to join a party
            Party party = _context.Parties.Where(p => p.JoinCode == joinCode).FirstOrDefault(); // Find a corresponding party with the requested join code
            if (party == null) // no party with that join code
            {
                throw new InvalidOperationException("A party with that code does not exist");
            }

            party.PartyMembers.Add(ToJoin);
            ToJoin.Party = party;

            _context.SaveChanges();

            return new JsonResult(new { Success = true });
        }

        /**
         * Func LeaveParty() -> ActionResult<String>
         *
         * Send a POST request to /api/party/leaveparty
         * The user that sent the request will be removed from the party which they are currently in
         *
         * 15/02/2020 D/M/Y - Nathan Tucker - Stubbing
         * 17/02/2020 D/M/Y - Nathan Tucker - Finalizing
         */
        [HttpPost]
        [Authorize]
        public ActionResult<String> LeaveParty()
        {
            _logger.LogDebug("User {0} attempting to leave party", _userManager.GetUserAsync(HttpContext.User).Result);

            User ToLeave = _userManager.GetUserAsync(HttpContext.User).Result;
            Party PartyToLeave = _context.Parties.Where(p => p == ToLeave.Party || p.Id == ToLeave.PartyId).FirstOrDefault();

            if (PartyToLeave == null)
            {
                throw new InvalidOperationException("Trying to leave party that does not exist");
            }

            PartyToLeave.PartyMembers.Remove(ToLeave);

            _context.SaveChanges();

            return new JsonResult(new { Success = true });
        }

        /**
         * Func DisbandParty -> ActionResult<String>
         *
         * Send a DELTE request to /api/party/disband
         * 
         * The current party will be removed from the database
         * All of its members will be left partyless
         * The host will be logged out and removed
         *
         * 17/02/2020 - Nathan Tucker - Stubbing
         */
        [HttpDelete]
        [Authorize]
        public ActionResult<String> DisbandParty()
        {
            User PartyDisbander = _userManager.GetUserAsync(HttpContext.User).Result;
            Party PartyToDisband = _context.Parties.Where(p => p.Id == PartyDisbander.PartyId).FirstOrDefault();

            if (PartyToDisband == null)
            {
                throw new InvalidOperationException("Attempting to dispand invalid party!");
            }

            if (PartyToDisband.PartyHost != PartyDisbander)
            {
                throw new InvalidOperationException("Attempting to disband party as non-host");
            }

            _context.Parties.Remove(PartyToDisband);

            _context.SaveChanges();

            return new JsonResult(new { Success = true });
        }
    }
}
