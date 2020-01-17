using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace cs309server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<String>> Index()
        {
            return new List<String> { "" };
        }
    }
}
