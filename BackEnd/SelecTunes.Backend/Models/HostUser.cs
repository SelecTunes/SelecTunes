using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models
{
    public class HostUser : User
    {
        [Required]
        public string SpotifyId { get; set; }
    }
}
