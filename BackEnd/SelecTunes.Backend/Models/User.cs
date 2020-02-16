using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Models
{
    public class User : IdentityUser
    {
        public string SpotifyAccessToken { get; set; }

        public string SpotifyRefreshToken { get; set; }

        public bool IsBanned { get; set; }
    }
}
