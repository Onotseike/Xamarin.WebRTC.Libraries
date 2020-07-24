// onotseike@hotmail.comPaula Aliu

using Newtonsoft.Json;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class ServerParameters
    {
        [JsonProperty("is_initiator")]
        public bool IsInitiator { get; set; }

        [JsonProperty("room_link")]
        public string RoomLink { get; set; }

        [JsonProperty("messages")]
        public string[] Messages { get; set; }

        [JsonProperty("error_messages")]
        public string[] ErrorMessages { get; set; }

        [JsonProperty("offer_options")]
        public string OfferOptions { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("ice_server_transports")]
        public string IceServerTransports { get; set; }

        [JsonProperty("bypass_join_confirmation")]
        public bool BypassJoinConfirmation { get; set; }

        [JsonProperty("media_constraints")]
        public string MediaConstraints { get; set; }

        [JsonProperty("include_loopback_js")]
        public string IncludeLoopbackJs { get; set; }

        [JsonProperty("is_loopback")]
        public bool IsLoopback { get; set; }

        [JsonProperty("wss_url")]
        public string WssUrl { get; set; }

        [JsonProperty("pc_constraints")]
        public string PcConstraints { get; set; }

        [JsonProperty("pc_config")]
        public string PcConfig { get; set; }

        [JsonProperty("wss_post_url")]
        public string WssPostUrl { get; set; }

        [JsonProperty("ice_server_url")]
        public string IceServerUrl { get; set; }

        [JsonProperty("warning_messages")]
        public string[] WarningMessages { get; set; }

        [JsonProperty("room_id")]
        public string RoomId { get; set; }
    }
}