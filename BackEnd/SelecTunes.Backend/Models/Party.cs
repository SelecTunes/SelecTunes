using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelecTunes.Backend.Models
{
    public class Party
    {
        public Party()
        {
            PartyMembers = new List<User>();
            SongQueue = new Queue<Song>();
        }

        [Key]
        public int Id { get; set; }

        public string JoinCode { get; set; }

        [NotMapped]
        public Queue<Song> SongQueue { get; set; }

        public virtual ICollection<User> PartyMembers { get; set; }

        public User PartyHost { get; set; }
        public string PartyHostId { get; set; }

        public virtual ICollection<User> KickedMembers { get; set; }

        public bool AllowExplicit { get; set; }
    }
}