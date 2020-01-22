using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Data;
using SelecTunes.Models;

namespace SelecTunes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private IDistributedCache _distributedCache;

        public IndexController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });

        [HttpGet]
        public ActionResult<String> Ack() {
            Party p = new Party
            {
                PartyHost = new HostUser
                {
                    Id = Guid.NewGuid(),
                    SpotifyId = "123"
                }
            };

            _context.Parties.Add(p);
            _context.SaveChanges();

            return Ok(p.ToString());
        }
    }
}
