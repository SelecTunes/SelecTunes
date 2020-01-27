using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SelecTunes.Backend.Models.SongSearchIngestion
{
    public class SpotifyTracksResponseBody
    {
        [JsonProperty("tracks")]
        public Tracks Tracks { get; set; }
    }

    public partial class Tracks
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("previous")]
        public object Previous { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("album")]
        public Album Album { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("disc_number")]
        public long DiscNumber { get; set; }

        [JsonProperty("duration_ms")]
        public long DurationMs { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("external_ids")]
        public ExternalIds ExternalIds { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_local")]
        public bool IsLocal { get; set; }

        [JsonProperty("is_playable")]
        public bool IsPlayable { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("popularity")]
        public long Popularity { get; set; }

        [JsonProperty("preview_url")]
        public Uri PreviewUrl { get; set; }

        [JsonProperty("track_number")]
        public long TrackNumber { get; set; }

        [JsonProperty("type")]
        public ItemType Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class Album
    {
        [JsonProperty("album_type")]
        public AlbumTypeEnum AlbumType { get; set; }

        [JsonProperty("artists")]
        public Artist[] Artists { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public ReleaseDate ReleaseDate { get; set; }

        [JsonProperty("release_date_precision")]
        public ReleaseDatePrecision ReleaseDatePrecision { get; set; }

        [JsonProperty("total_tracks")]
        public long TotalTracks { get; set; }

        [JsonProperty("type")]
        public AlbumTypeEnum Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class Artist
    {
        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public ArtistType Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    public partial class ExternalUrls
    {
        [JsonProperty("spotify")]
        public Uri Spotify { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("height")]
        public long Height { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }
    }

    public partial class ExternalIds
    {
        [JsonProperty("isrc")]
        public string Isrc { get; set; }
    }

    public enum AlbumTypeEnum { Album, Compilation, Single };

    public enum ArtistType { Artist };

    public enum ReleaseDatePrecision { Day, Year };

    public enum ItemType { Track };

    public partial struct ReleaseDate
    {
        public DateTimeOffset? DateTime;
        public long? Integer;

        public static implicit operator ReleaseDate(DateTimeOffset DateTime) => new ReleaseDate { DateTime = DateTime };
        public static implicit operator ReleaseDate(long Integer) => new ReleaseDate { Integer = Integer };
    }
}
