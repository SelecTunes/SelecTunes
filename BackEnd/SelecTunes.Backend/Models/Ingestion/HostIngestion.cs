using System;
using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models.Ingestion
{
    public class HostIngestion
    {
        [Required]
        public string SpotifyHash { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
