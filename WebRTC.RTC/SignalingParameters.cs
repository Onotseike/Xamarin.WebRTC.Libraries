// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;
using WebRTC.RTC.Abstraction;

namespace WebRTC.RTC
{
    public class SignalingParameters : ISignalingParameters
    {
        public IceServer[] IceServers { get; set; }
        public bool IsInitiator { get; set; }
        public string ClientId { get; set; }
        public string WssUrl { get; set; }
        public string WssPostUrl { get; set; }
        public SessionDescription OfferSdp { get; set; }
        public IceCandidate[] IceCandidates { get; set; }
    }
}