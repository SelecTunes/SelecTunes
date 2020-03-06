using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace SelecTunes.Backend.Models.Auth
{
    // These are the access token spotify returns to us from the user.
    //{"access_token":"...","token_type":"Bearer","expires_in":3600,"refresh_token":"...","scope":"user-read-email user-read-private"}
    [Owned]
    public class AccessAuthToken
    {
        public AccessAuthToken()
        {
            CreateDate = DateTime.Now;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsExpired()
        {
            return CreateDate.Add(TimeSpan.FromSeconds(ExpiresIn)) <= DateTime.Now;
        }
    }
}
