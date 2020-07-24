// onotseike@hotmail.comPaula Aliu


using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class RTCController : RTCControllerBase<RoomConnectionParameters, SignalingParameters>
    {
        public RTCController(IRTCEngineEvents _events, ILogger _logger = null) : base(_events, _logger)
        {

        }


        #region  Implements

        protected override bool IsInitiator => SignalParameters.IsInitiator;
        protected override IRTCClient<RoomConnectionParameters> CreateClient() => new RTCClient(this);

        protected override PeerConnectionParameters CreatePeerConnectionParameters(SignalingParameters signalingParameters)
        {
            var signalingParam = signalingParameters;
            return new PeerConnectionParameters(signalingParam.IceServers)
            {
                IsVideoCallEnabled = true
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

        #endregion
    }
}
