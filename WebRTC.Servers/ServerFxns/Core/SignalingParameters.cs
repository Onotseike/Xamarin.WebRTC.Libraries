// onotseike@hotmail.comPaula Aliu

using WebRTC.Classes;
using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class SignalingParameters : ISignalingParameters
    {
        public IceServer[] IceServers { get; set; }
        public bool IsInitiator { get; set; }
        public string ClientId { get; set; }
        public string WssURL { get; set; }
        public string WssPostURL { get; set; }
        public SessionDescription OfferSdp { get; set; }
        public IceCandidate[] IceCandidates { get; set; }
    }
}