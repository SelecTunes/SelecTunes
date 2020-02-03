using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models.SongSearchIngestion
{
    public class SearchQuery
    {
        [Required]
        public string QueryString { get; set; }
    }
}
