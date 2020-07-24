// onotseike@hotmail.comPaula Aliu

using Newtonsoft.Json;

using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core.JsonObjects
{
    public class JoinResponseHelper
    {
        [JsonProperty("params")]
        public ServerParameters ServerParameters { get; set; }

        [JsonConverter(typeof(JoinResultTypeStringConverter))]
        [JsonProperty("result")]
        public JoinResponseType Result { get; set; }
    }
}