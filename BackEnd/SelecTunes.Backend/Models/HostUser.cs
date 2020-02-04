using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models
{
    public class HostUser : User
    {
        [Required]
        public string SpotifyAccessToken { get; set; }

        [Required]
        public string SpotifyRefreshToken { get; set; }

        [Required]
        public string Email { get; set; }


    }
}
