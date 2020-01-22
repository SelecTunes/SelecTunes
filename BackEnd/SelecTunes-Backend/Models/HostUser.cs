using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Models
{
    public class HostUser : User
    {
        public string SpotifyId { get; set; }
    }
}
