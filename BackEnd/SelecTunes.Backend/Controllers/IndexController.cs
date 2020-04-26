using Microsoft.AspNetCore.Mvc;

namespace SelecTunes.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        /**
         * Func Index() -> async <ActionResult<List<String>>>
         * => List { "" }
         * 
         * Returns an Empty List
         * 
         * 16/02/2020 D/M/Y - Alexander Young - Cleanup
         */
        [HttpGet]
        public RedirectResult Index() => Redirect("/swagger/index.html");
    }
}
