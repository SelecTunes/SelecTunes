using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Models
{
    public class Party
    {

        public Party (string HostPhoneNumber, string UserName)
        {
            PhoneNumber = HostPhoneNumber;
            SongQueue = new Queue<Song>();
            PartyMembers = new List<User>();
            BannedMembers = new List<User>();

            PartyHost = new User(HostPhoneNumber, true, UserName);
        }

        public string Name { get; set; }

        public int PartyUID { get; set; }

        public Queue<Song> SongQueue { get; set; }

        public List<User> PartyMembers { get; set; }

        public List<User> BannedMembers { get; set; }

        public User PartyHost { get; set; }

        public string PhoneNumber { get; set; }

        
    }
}