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
        public ActionResult<List<String>> Index() => Ok(new List<String> { "" });

        /*
         * This is a ton of example code we wrote for redis and ClientFactory
         * 
         * To be used in other method calls such as creating parties, guests, etc.
         */
        [HttpGet]
        public async Task<ActionResult<int>> Ack() {
            throw new NotImplementedException("This code has not yet been implemented");

            Random random = new Random();
            Party p = new Party
            {
                // Create a new PartyHost with the following values
                PartyHost = new User
                {
                    UserName = random.Next().ToString(),
                    PhoneNumber = random.Next().ToString(),
                    IsBanned = false,
                },
            };


            _context.Parties.Add(p); // Add a party to the database set of all parties we know about
            _context.SaveChanges(); // Commit to database

            String serial = JsonConvert.SerializeObject(p); // Serialize the redis entries, as we only have one cache

            _cache.SetString($"$party:${p.Id}", serial, new DistributedCacheEntryOptions()); // Default never expire //TO TEST LATER

            using (HttpClient c = _cf.CreateClient("spotify")) // create a new client to spotify, see Startup
            {
                using HttpRequestMessage r = new HttpRequestMessage(HttpMethod.Get, "me/player/devices"); // go to that route. TODO error handling, pass to phone
                HttpResponseMessage s = await c.SendAsync(r).ConfigureAwait(false);

                Console.WriteLine(await s.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            return Ok(p.Id); // 200
        }
    }
}
