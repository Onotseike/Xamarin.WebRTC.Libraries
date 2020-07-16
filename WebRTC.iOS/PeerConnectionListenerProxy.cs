// onotseike@hotmail.comPaula Aliu
using System.Linq;
using Foundation;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal class PeerConnectionListenerProxy : NSObject, IRTCPeerConnectionDelegate
    {
        private readonly IPeerConnectionListener _listener;

        public PeerConnectionListenerProxy(IPeerConnectionListener listener)
        {
            _listener = listener;
        }

        public void DidChangeSignalingState(RTCPeerConnection peerConnection, RTCSignalingState stateChanged)
        {
            _listener?.OnSignalingChange(stateChanged.ToNet());
        }

        public void DidAddStream(RTCPeerConnection peerConnection, RTCMediaStream stream)
        {
            _listener?.OnAddStream(new MediaStreamNative(stream));
        }

        public void DidRemoveStream(RTCPeerConnection peerConnection, RTCMediaStream stream)
        {
            _listener?.OnRemoveStream(new MediaStreamNative(stream));
        }

        public void PeerConnectionShouldNegotiate(RTCPeerConnection peerConnection)
        {
            _listener?.OnRenegotiationNeeded();
        }

        public void DidChangeIceConnectionState(RTCPeerConnection peerConnection, RTCIceConnectionState newState)
        {
            _listener?.OnIceConnectionChange(newState.ToNet());
        }

        public void DidChangeIceGatheringState(RTCPeerConnection peerConnection, RTCIceGatheringState newState)
        {
            _listener?.OnIceGatheringChange(newState.ToNet());
        }

        public void DidGenerateIceCandidate(RTCPeerConnection peerConnection, RTCIceCandidate candidate)
        {
            _listener?.OnIceCandidate(candidate.ToNet());
        }

        public void DidRemoveIceCandidates(RTCPeerConnection peerConnection, RTCIceCandidate[] candidates)
        {
            _listener?.OnIceCandidatesRemoved(candidates.ToNet().ToArray());
        }

        public void DidOpenDataChannel(RTCPeerConnection peerConnection, RTCDataChannel dataChannel)
        {
            _listener?.OnDataChannel(new DataChannelNative(dataChannel));
        }
    }
}