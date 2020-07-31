// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.RTC.Abstraction;

namespace WebRTC.RTC
{
    public class RTCController : RTCControllerBase<RoomConnectionParameters, SignalingParameters>
    {
        public RTCController(IRTCEngineEvents events, ILogger logger = null) : base(events, logger)
        {
        }

        protected override bool IsInitiator => SignalingParameters.IsInitiator;
        protected override IRTCClient<RoomConnectionParameters> CreateClient() => new RTCClient(this);

        protected override PeerConnectionParameters CreatePeerConnectionParameters(SignalingParameters signalingParameters)
        {
            var signalingParam = signalingParameters;
            return new PeerConnectionParameters(signalingParam.IceServers)
            {
                VideoCallEnabled = true
            };
        }

        protected override void OnChannelConnectedInternal(SignalingParameters signalingParameters)
        {
            OnConnectedToRoomInternal(signalingParameters);
        }

        private void OnConnectedToRoomInternal(SignalingParameters signalingParameters)
        {
            if (signalingParameters.IsInitiator)
            {
                PeerConnectionClient.CreateOffer();
            }
            else
            {
                if (signalingParameters.OfferSdp != null)
                {
                    PeerConnectionClient.SetRemoteDescription(signalingParameters.OfferSdp);
                    PeerConnectionClient.CreateAnswer();
                }

                if (signalingParameters.IceCandidates != null)
                {
                    foreach (var candidate in signalingParameters.IceCandidates)
                    {
                        PeerConnectionClient.AddRemoteIceCandidate(candidate);
                    }
                }
            }
        }
    }
}
