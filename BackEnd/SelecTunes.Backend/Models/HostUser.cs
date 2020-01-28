using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models
{
    public class HostUser : User
    {
        [Required]
        public string SpotifyHash { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
