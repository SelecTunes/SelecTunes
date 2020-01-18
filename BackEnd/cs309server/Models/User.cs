using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cs309server.Models
{
    public class User
    {
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
    }
}
