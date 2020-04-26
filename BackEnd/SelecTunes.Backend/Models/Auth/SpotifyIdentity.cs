using Newtonsoft.Json;

namespace SelecTunes.Backend.Models.Auth
{
    public class SpotifyIdentity
    {
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }
    }
}
