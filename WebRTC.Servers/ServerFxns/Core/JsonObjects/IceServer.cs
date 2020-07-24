// onotseike@hotmail.comPaula Aliu
using System;

using Newtonsoft.Json;

namespace WebRTC.Servers.ServerFxns.Core.JsonObjects
{
    public class IceServer
    {
        [JsonProperty("urls")]
        public string[] Urls { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("credential")]
        public string Credential { get; set; }

        [JsonProperty("maxRateKbps")]
        public string MaxRateKbps { get; set; }
    }
}
