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

    public partial class Devices
    {
        [JsonProperty("devices")]
        public List<Device> Ope { get; set; }
    }

    public partial class Device
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_private_session")]
        public bool IsPrivateSession { get; set; }

        [JsonProperty("is_restricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("volume_percent")]
        public long VolumePercent { get; set; }
    }
}
