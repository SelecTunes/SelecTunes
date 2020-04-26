using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SelecTunes.Backend.Models.SongSearchIngestion
{
    public partial class SpotifyTracksResponseBody
    {
        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }
    }

    public partial class Tracks
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public ICollection<TrackItem> Items { get; set;  }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public partial class TrackItem
    {
        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("artists")]
        public ICollection<Artist> Artists { get; set; }

        [JsonProperty("disc_number")]
        public int DiscNumber { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("preview_url")]
        public Uri PreviewUrl { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class Album
    { 
        [JsonProperty("artists")]
        public ICollection<Artist> Artists { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public ICollection<TrackImage> Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Artist
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class TrackImage
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
