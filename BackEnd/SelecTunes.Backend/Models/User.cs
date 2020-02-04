using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelecTunes.Backend.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string UserName { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        public bool IsBanned { get; set; }
    }
}
