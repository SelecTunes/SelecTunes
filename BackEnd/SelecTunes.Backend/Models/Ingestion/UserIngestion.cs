using System;
using System.ComponentModel.DataAnnotations;

namespace SelecTunes.Backend.Models.Ingestion
{
    public class UserIngestion
    {
        [Required]
        public string PhoneNumber { get; set; }

        public Int32 JoinCode { get; set; }
    }
}
