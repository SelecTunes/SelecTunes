using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Models
{
    public class User
    {

        public User(string PhoneNumber, bool IsHost, string UserName)
        {
            this.PhoneNumber = PhoneNumber;
            this.IsHost = IsHost;
            this.UserName = UserName;
        }

        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBanned { get; set; }
        public bool IsHost { get; set; }
    }
}
