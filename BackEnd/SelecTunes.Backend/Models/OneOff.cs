using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Models.OneOff
{
    public partial class JoinRequest
    {
        public string JoinCode { get; set; }
    }

    public partial class GetDeleteThis
    {
        public string Id { get; set; }
    }

    public partial class PesudoUser
    {
        public string Email { get; set; }
    }
}
