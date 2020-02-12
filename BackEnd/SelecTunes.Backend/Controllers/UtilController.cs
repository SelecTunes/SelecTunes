using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        [HttpGet]
        public ActionResult<String> Version()
        {
            return typeof(Program).Assembly.GetName().Version.ToString();
        }
    }
}