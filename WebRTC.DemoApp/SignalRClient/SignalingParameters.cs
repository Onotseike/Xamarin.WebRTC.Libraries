// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Classes;
using WebRTC.DemoApp.SignalRClient.Abstraction;

namespace WebRTC.DemoApp.SignalRClient
{
    public class SignalingParameters : ISignalingParameters
    {
        public IceServer[] IceServers { get; set; }
        public bool IsInitiator { get; set; }
        public string ClientId { get; set; }
        public string ClientUsername { get; set; }
        //public string WssUrl { get; set; }
        //public string WssPostUrl { get; set; }
        public SessionDescription OfferSdp { get; set; }
        public IceCandidate[] IceCandidates { get; set; }
    }
}
