using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SelecTunes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });
    }
}
