using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SelecTunes.Backend.Models;
using SelecTunes.Backend.Models.OneOff;
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

        public async Task<bool> SendToSpotifyQueue(User user, string id, Device device = null)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!id.StartsWith("spotify:track:", StringComparison.InvariantCulture))
            {
                id = $"spotify:track:{id}";
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

            if (device != null)
            {
                formContent.Add("device_id", device.Id);
            }

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

        public async Task<Device> GetRandomDevice(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            HttpClient c = ClientFactory.CreateClient("spotify");

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                user.Token.AccessToken
            );

            HttpResponseMessage r = await c.GetAsync(new Uri("me/player/devices", UriKind.Relative)).ConfigureAwait(false);

            if (!r.IsSuccessStatusCode)
            {
                Console.WriteLine(await r.Content.ReadAsStringAsync().ConfigureAwait(false));
                return null;
            }

            Devices devices = JsonConvert.DeserializeObject<Devices>(await r.Content.ReadAsStringAsync().ConfigureAwait(false));

            if (devices.Ope == null || devices.Ope.Count == 0)
            {
                Console.WriteLine("No devices returned from spotify");
                return null;
            }

            Device device = devices.Ope.FirstOrDefault(x => x.IsActive);

            if (device == null)
            {
                Random x = new Random();
                device = devices.Ope[x.Next(devices.Ope.Count)];
            }

            return device;
        }

        public async Task<bool> BeginPlayback(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            HttpClient c = ClientFactory.CreateClient("spotify");

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                user.Token.AccessToken
            );

            Device device = await GetRandomDevice(user).ConfigureAwait(false);

            if (device == null)
            {
                return false;
            }

            if (!await SendToSpotifyQueue(user, "spotify:track:4uLU6hMCjMI75M1A2tKUQC", device).ConfigureAwait(false))
            {
                return false;
            }

            Dictionary<string, string> formContent = new Dictionary<string, string>
            {
                {"device_id", device.Id},
            };

            string query = QueryHelpers.AddQueryString("me/player/play", formContent);

            HttpResponseMessage r = await c.PutAsync(query, null).ConfigureAwait(false);

            if (!r.IsSuccessStatusCode)
            {
                Console.WriteLine(await r.Content.ReadAsStringAsync().ConfigureAwait(false));
                return true;
            }

            return true;
        }
    }
}
