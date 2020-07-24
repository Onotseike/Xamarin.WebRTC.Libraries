// onotseike@hotmail.comPaula Aliu

using Newtonsoft.Json;

namespace WebRTC.Servers.ServerFxns.Core.JsonObjects
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
    }
}