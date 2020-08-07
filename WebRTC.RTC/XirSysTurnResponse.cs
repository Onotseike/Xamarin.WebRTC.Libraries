// onotseike@hotmail.comPaula Aliu
using System;

using Newtonsoft.Json;

namespace WebRTC.RTC
{
    public class XirSysTurnResponse
    {
        [JsonProperty("lifetimeDuration")]
        public string LifetimeDuration { get; set; }

        [JsonProperty("iceServers")]
        public XirSysIceServer[] IceServers { get; set; }

        [JsonProperty("blockStatus")]
        public string BlockStatus { get; set; }

        [JsonProperty("iceTransportPolicy")]
        public string IceTransportPolicy { get; set; }
    }
}
