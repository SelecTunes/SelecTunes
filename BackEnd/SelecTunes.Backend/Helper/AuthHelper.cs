using Newtonsoft.Json;
using SelecTunes.Backend.Models.Auth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Helper
{
    public class AuthHelper
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public IHttpClientFactory ClientFactory { get; set; }

        public string RedirectUrl { get; set; }

        public async Task<AccessAuthToken> TransmutAuthCode(string code)
        {
            if (code == null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (code.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(code));
            }

            using (HttpClient c = ClientFactory.CreateClient("spotify-accounts")) // create a new client to spotify, see Startup
            {
                string clientHeader = Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(
                        $"{ClientId}:{ClientSecret}"
                    )
                );

                // Add the Authorization Header, now that it exists.
                c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    clientHeader
                );

                using FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    new KeyValuePair<string, string>("redirect_uri", RedirectUrl),
                    new KeyValuePair<string, string>("code", code)
                });
                HttpResponseMessage r = await c.PostAsync(
                    new Uri("token", UriKind.Relative),
                    formContent
                ).ConfigureAwait(false);

                string s = await r.Content.ReadAsStringAsync().ConfigureAwait(false);

                AccessAuthToken content = JsonConvert.DeserializeObject<AccessAuthToken>(s);

                return content;
            }
        }

        public async Task<AccessAuthToken> AssertValidLogin(AccessAuthToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token.RefreshToken == null || token.RefreshToken.Length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(token));
            }

            using HttpClient c = ClientFactory.CreateClient("spotify-accounts");

            while (true)
            {
                using FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", token.RefreshToken)
                });

                HttpResponseMessage response = await c.PostAsync(
                    new Uri("token", UriKind.Relative),
                    formContent
                ).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    AccessAuthToken tok = JsonConvert.DeserializeObject<AccessAuthToken>(responseString);
                    tok.RefreshToken = token.RefreshToken;

                    return tok;
                }

                Thread.Sleep(500);
            }
        }

        public string TryLogin(string x)
        {
            return x;
        }
    }
}
