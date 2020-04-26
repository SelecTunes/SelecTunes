using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Helper.Exceptions;
using SelecTunes.Backend.Models;
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
            { // If the code is null, throw a Arg Nil.
                throw new ArgumentNullException(nameof(code));
            }

            if (code.Length <= 0)
            { // If the code is empty, throw a Arg Out of Range.
                throw new ArgumentOutOfRangeException(nameof(code));
            }

            using HttpClient c = ClientFactory.CreateClient("spotify-accounts");
            string clientHeader = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    $"{ClientId}:{ClientSecret}"
                )
            ); // Encode the Client ID and Client Secret to Base 64

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                clientHeader
            ); // Add the Authorization Header, now that it exists.

            using FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
            { // See https://developer.spotify.com/documentation/general/guides/authorization-guide/
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", RedirectUrl),
                new KeyValuePair<string, string>("code", code),
            }); // Create a new FormContent to give to spotify.

            HttpResponseMessage r = await c.PostAsync(
                new Uri("token", UriKind.Relative), // This is a Uri object instead of a string so VS can stop complaining.
                formContent
            ).ConfigureAwait(false); // Post it over to spotify.

            string s = await r.Content.ReadAsStringAsync().ConfigureAwait(false); // Await the result response.

            AccessAuthToken content = JsonConvert.DeserializeObject<AccessAuthToken>(s); // Deserialize it to a AccessToken.

            return content;
        }

        public async Task<AccessAuthToken> AssertValidLogin(AccessAuthToken token, Boolean loopUntilSuccess)
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

            string clientHeader = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(
                    $"{ClientId}:{ClientSecret}"
                )
            ); // Encode the Client ID and Client Secret to Base 64

            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                clientHeader
            ); // Add the Authorization Header, now that it exists.

            do
            { // While the token is invalid, repeat.
                using FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                { // See https://developer.spotify.com/documentation/general/guides/authorization-guide/
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", token.RefreshToken)
                }); // Create a new FormContent to send over to spotify.

                HttpResponseMessage response = await c.PostAsync(
                    new Uri("token", UriKind.Relative), // This is a Uri object instead of a string so VS can stop complaining.
                    formContent
                ).ConfigureAwait(false); // Post it over to spotify.

                String x = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                { // If the response is successful, deserialize it to a AccessToken and return that.
                    string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    AccessAuthToken tok = JsonConvert.DeserializeObject<AccessAuthToken>(responseString);
                    tok.RefreshToken = token.RefreshToken;

                    return tok;
                }

                // If not successful, sleep for 500 milliseconds, and retry.
                Thread.Sleep(500);
            } while (loopUntilSuccess); // POTENTIAL INFINITE LOOP CONDITION. If spotify never returns a succesful response AND loopUntilSuccess is true, this code will loop forever.

            throw new InvalidAuthTokenRecievedException("Unable to assert authentication is valid");
        }

        public async Task<User> UpdateUserTokens(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!user.Token.IsExpired())
            {
                return user;
            }

            user.Token = await AssertValidLogin(user.Token, false).ConfigureAwait(false);

            return user;
        }

        public bool BanUser(User ToBan, User CurrentUser, ApplicationContext context)
        {
            if(ToBan == null)
            {
                return false;
            }

            if(CurrentUser == null)
            {
                return false;
            }

            if (context == null)
            {
                return false;
            }
            // Reset strike counter
            ToBan.Strikes = 0;
            // Do all of the lockout features
            ToBan.IsBanned = true;
            ToBan.LockoutEnabled = true;
            ToBan.LockoutEnd = DateTimeOffset.Now.AddDays(16);
            ToBan.Party = null;
            ToBan.PartyId = null;
            CurrentUser.Party.PartyMembers.Remove(ToBan);

            context.SaveChanges();

            return true;
        }

        public static string ParseIdentityResult(IdentityResult identityResult)
        {
            if (identityResult == null)
            {
                throw new NullReferenceException("identity result is null");
            }

            if (identityResult.Succeeded)
            {
                return "Success";
            }

            foreach (IdentityError err in identityResult.Errors)
            {
                if (err.Description.Contains("match", StringComparison.Ordinal))
                {
                    return "Passwords do not match";
                }
                else if (err.Description.Contains("long", StringComparison.Ordinal))
                {
                    return "Password does not meet length requirement";
                }
            }

            return null;
        }
    }
}
