using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models.Auth
{
    public class SpotifyLogin
    {
        [Required]
        public string Code { get; set; }
    }
}
