// onotseike@hotmail.comPaula Aliu
using Newtonsoft.Json;

namespace WebRTC.RTC
{
    public class TurnResponse
    {
        [JsonProperty("lifetimeDuration")]
        public string LifetimeDuration { get; set; }

        [JsonProperty("iceServers")]
        public IceServer[] IceServers { get; set; }

        [JsonProperty("blockStatus")]
        public string BlockStatus { get; set; }

        [JsonProperty("iceTransportPolicy")]
        public string IceTransportPolicy { get; set; }

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
}