using Microsoft.AspNetCore.Mvc;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Version() => typeof(Program).Assembly.GetName().Version.ToString();
    }
}