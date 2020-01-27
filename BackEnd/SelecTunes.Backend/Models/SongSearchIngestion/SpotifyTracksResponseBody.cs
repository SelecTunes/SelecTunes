using System.Collections.Generic;
using Newtonsoft.Json;

namespace SelecTunes.Backend.Models.SongSearchIngestion
{
    public class SpotifyTracksResponseBody
    {
        //[JsonProperty("tracks")]
        private List<Track> Tracks { get; set; }

        private class Track
        {
            //[JsonProperty("href")]
            private string HRef { get; set; }

            //[JsonProperty("items")]
            private List<Item> Items { get; set; }

            //[JsonProperty("limit")]
            private int Limit { get; set; }

            //[JsonProperty("next")]
            private string Next { get; set; }

            //[JsonProperty("offset")]
            private int Offset { get; set; }

            //[JsonProperty("previous")]
            private string Previous { get; set; }

            //[JsonProperty("total")]
            private int Total { get; set; }

            private class Item
            {
                //[JsonProperty("album")]
                private Album ItemAlbum { get; set; }

                //[JsonProperty("artists")]
                private List<Artist> Artists { get; set; }

                [JsonProperty("disc_number")]
                private int DiscNumber { get; set; }

                [JsonProperty("duration_ms")]
                private int DurationMs { get; set; }

                [JsonProperty("explicit")]
                private bool Explicit;

                [JsonProperty("external_ids")]
                private ExternalIds ExternalIds { get; set; }

                [JsonProperty("external_urls")]
                private ExternalUrls ExternalUrls { get; set; }

                //[JsonProperty("href")]
                private string HRef { get; set; }

                //[JsonProperty("id")]
                private string ID { get; set; }

                [JsonProperty("is_local")]
                private bool IsLocal;

                [JsonProperty("is_playable")]
                private bool IsPlayable;

                //[JsonProperty("name")]
                private string Name { get; set; }

                [JsonProperty("popularity")]
                private int Popularity { get; set; }

                [JsonProperty("preview_url")]
                private string PreviewUrl { get; set; }

                [JsonProperty("track_number")]
                private int TrackNumber { get; set; }

                //[JsonProperty("type")]
                private string Type { get; set; }

                //[JsonProperty("uri")]
                private string URI { get; set; }

                private class Album
                {
                    [JsonProperty("album_type")]
                    private string AlbumType { get; set; }

                    [JsonProperty("artists")]
                    private List<Artist> ArtistList { get; set; }
                }
            }

        }
    }
}
