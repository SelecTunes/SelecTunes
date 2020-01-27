using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        private readonly IHttpClientFactory _cf;

        public IndexController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory)
        {
            _context = context;
            _cache = cache;
            _cf = factory;
        }

        [HttpGet]
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });

        /*
         * This is a ton of example code we wrote for redis and ClientFactory
         * 
         * To be used in other method calls such as creating parties, guests, etc.
         */
        [HttpGet]
        public async Task<ActionResult<int>> Ack() {
            Random random = new Random();
            Party p = new Party
            {
                // Create a new PartyHost with the following values
                PartyHost = new HostUser
                {
                    SpotifyHash = random.Next().ToString(),
                    UserName = random.Next().ToString(),
                    PhoneNumber = random.Next().ToString(),
                    IsBanned = false,
                    IsHost = false
                },
            };


            _context.Parties.Add(p); // Add a party to the database set of all parties we know about
            _context.SaveChanges(); // Commit to database

            String serial = JsonConvert.SerializeObject(p); // Serialize the redis entries, as we only have one cache

            _cache.SetString($"$party:${p.Id}", serial, new DistributedCacheEntryOptions()); // Default never expire //TO TEST LATER

            using (HttpClient c = _cf.CreateClient("spotify")) // create a new client to spotify, see Startup
            {
                using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, "me/player/devices"); // go to that route. TODO error handling, pass to phone
                HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

                Console.WriteLine(await s.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            return Ok(p.Id); // 200
        }
    }
}
