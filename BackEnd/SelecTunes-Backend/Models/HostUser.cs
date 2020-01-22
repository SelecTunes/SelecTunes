using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Models
{
    public class HostUser : User
    {
        [Required]
        public string SpotifyId { get; set; }
    }
}
