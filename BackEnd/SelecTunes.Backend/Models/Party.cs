using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelecTunes.Backend.Models
{
    public class Party
    {
        [Key]
        public int Id { get; set; }

        public string JoinCode { get; set; }

        [NotMapped]
        public Queue<Song> SongQueue { get; set; }

        [NotMapped]
        public List<User> PartyMembers { get; set; }

        public User PartyHost { get; set; }
    }
}