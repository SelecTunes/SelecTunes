using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SelecTunes.Backend.Models.Auth;

namespace SelecTunes.Backend.Models
{
    public class User : IdentityUser
    {
        public bool IsBanned { get; set; }

        public Party Party { get; set; }
        public int? PartyId { get; set; }

        public AccessAuthToken Token { get; set; }

        public int Strikes { get; set; }

    }
}
