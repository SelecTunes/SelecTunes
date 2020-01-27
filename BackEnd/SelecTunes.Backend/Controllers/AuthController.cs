using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelecTunes.Backend.Models.Ingestion;
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
        public ActionResult<RedirectResult> Login([FromBody]String newHost)
        {
            if (newHost == null)
            {
                return new BadRequestObjectResult("Object is null");
            }

            Console.WriteLine(newHost);

            HostIngestion ingestionHost = JsonConvert.DeserializeObject<HostIngestion>(newHost);


            return RedirectToAction("Index");
        }
        
    }
}
