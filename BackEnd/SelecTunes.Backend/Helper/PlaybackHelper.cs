using Microsoft.AspNetCore.WebUtilities;
using SelecTunes.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper
{
    public class PlaybackHelper
    {
        public IHttpClientFactory ClientFactory { get; set; }

        public async Task<bool> SendToSpotifyQueue(User user, string id)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            HttpClient c = ClientFactory.CreateClient("spotify");

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                user.Token.AccessToken
            );

            c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Dictionary<string, string> formContent = new Dictionary<string, string>
            { 
                {"uri", id},
            };

            string query = QueryHelpers.AddQueryString("me/player/queue", formContent);

            /*using StringContent content = new StringContent(null, null, "application/json");*/

            HttpResponseMessage response = await c.PostAsync(
                query,
                null
            ).ConfigureAwait(false); // Post it over to spotify.

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                return false;
            }

            return true;
        }

        public async Task<bool> BeginPlayback(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            HttpClient c = ClientFactory.CreateClient("spotify");

            if (!await SendToSpotifyQueue(user, "spotify:track:4uLU6hMCjMI75M1A2tKUQC").ConfigureAwait(false))
            {
                return false;
            }

            HttpResponseMessage r = await c.PutAsync(new Uri("me/player/play", UriKind.Relative), null).ConfigureAwait(false);

            if (!r.IsSuccessStatusCode)
            {
                Console.WriteLine(await r.Content.ReadAsStringAsync().ConfigureAwait(false));
                return false;
            }

            return true;
        }
    }
}
