using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public bool IsHost { get; set; }
    }
}
