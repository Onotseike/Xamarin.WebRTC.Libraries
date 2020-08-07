// onotseike@hotmail.comPaula Aliu
using Newtonsoft.Json;

namespace WebRTC.RTC
{
    public class XirSysIceServer
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("credential")]
        public string Credential { get; set; }

        [JsonProperty("maxRateKbps")]
        public string MaxRateKbps { get; set; }
    }
}