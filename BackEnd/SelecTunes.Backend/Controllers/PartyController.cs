using System;
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

        [HttpPost]
        public ActionResult<String> JoinParty([FromBody]User newUser)
        {
            throw new NotImplementedException("This code has not yet been implemented");

            if (newUser == null)
            {
                return new BadRequestObjectResult("Object is null");
            }
            Console.WriteLine(JsonConvert.SerializeObject(newUser));

            // String serial = JsonConvert.SerializeObject(p); // Serialize the redis entries, as we only have one cache

            // _cache.SetString($"$party:${p.Id}", serial, new DistributedCacheEntryOptions()); // Default never expire //TO TEST LATER

            String serial = JsonConvert.SerializeObject(newUser);

            _cache.SetString($"$party:${newUser.PhoneNumber}", serial, new DistributedCacheEntryOptions());

            return Ok("Index");
        }

        [HttpPost]
        public ActionResult<String> LeaveParty([FromBody]User toLeave)
        {
            throw new NotImplementedException("This code has not yet been implemented");

            if (toLeave == null)
            {
                return new BadRequestObjectResult("Object is null");
            }

            Console.WriteLine(JsonConvert.SerializeObject(toLeave));

            return Ok("Index");
        }
    }
}
