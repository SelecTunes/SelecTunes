using System;
using System.Collections.Generic;
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

        public IndexController(ApplicationContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet]
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });

        [HttpGet]
        public ActionResult<int> Ack() {
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

            return Ok(p.Id);
        }
    }
}
