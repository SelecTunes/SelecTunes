using Microsoft.AspNetCore.Identity;
using SelecTunes.Backend.Models.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Models
{
    public class User : IdentityUser
    {
        public bool IsBanned { get; set; }

        public Party Party { get; set; }
        public int? PartyId { get; set; }

        public AccessAuthToken Token { get; set; }
    }
}
