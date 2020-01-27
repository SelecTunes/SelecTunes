using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IDistributedCache _cache;

        public AuthController(ApplicationContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost]
        public ActionResult<String> Login([FromBody]HostUser newHost)
        {
            if (newHost == null)
            {
                return new BadRequestObjectResult("Object is null");
            }

            Console.WriteLine(JsonConvert.SerializeObject(newHost));

            return Ok("Index");
        }
        
    }
}
