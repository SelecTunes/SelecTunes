using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SelecTunes.Data;
using SelecTunes.Models;

namespace SelecTunes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private IDistributedCache _cache;

        private IHttpClientFactory _cf;

        public IndexController(ApplicationContext context, IDistributedCache cache, IHttpClientFactory factory)
        {
            _context = context;
            _cache = cache;
            _cf = factory;
        }

        [HttpGet]
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });

        [HttpGet]
        public async Task<ActionResult<int>> Ack() {
            Random random = new Random();
            Party p = new Party
            {
                PartyHost = new HostUser
                {
                    Id = Guid.NewGuid(),
                    SpotifyId = random.Next().ToString(),
                    UserName = random.Next().ToString(),
                    PhoneNumber = random.Next().ToString(),
                    IsBanned = false,
                    IsHost = true
                },
                Name = random.Next().ToString(),
                Id = random.Next()
            };

            _context.Parties.Add(p);
            _context.SaveChanges();

            String serial = JsonConvert.SerializeObject(p);

            _cache.SetString($"$party:${p.Id}", serial, new DistributedCacheEntryOptions());

            using (HttpClient c = _cf.CreateClient("spotify"))
            {
                HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, "me/player/devices");

                HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

                Console.WriteLine(await s.Content.ReadAsStringAsync().ConfigureAwait(false));
                r.Dispose();
            }

            return Ok(p.Id);
        }
    }
}
