// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class PCConfig
    {
        [JsonProperty("rtcpMuxPolicy")]
        public string RtcpMuxPolicy { get; set; }

        [JsonProperty("bundlePolicy")]
        public string BundlePolicy { get; set; }

        [JsonProperty("iceServers")]
        public IList<string> IceServers { get; set; }
    }
}
