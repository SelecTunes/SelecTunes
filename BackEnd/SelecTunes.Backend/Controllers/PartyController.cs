using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        public PartyController(ApplicationContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
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
            throw new NotImplementedException("This code has not yet been implemented");
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
