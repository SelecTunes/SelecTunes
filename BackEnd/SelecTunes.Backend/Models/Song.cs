using Newtonsoft.Json;

namespace SelecTunes.Backend.Models
{
    public class Song
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("artist_name")]
        public string ArtistName { get; set; }

        [JsonProperty("album_art_url")]
        public string AlbumArt { get; set; }
    }
}