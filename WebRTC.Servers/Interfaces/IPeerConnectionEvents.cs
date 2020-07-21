// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Classes;
using WebRTC.Interfaces;

namespace WebRTC.Servers.Interfaces
{
    public interface IPeerConnectionEvents
    {
        void OnPeerFactoryCreated(IPeerConnectionFactory factory);

        void OnPeerConnectionCreated(IPeerConnection peerConnection);
        void OnPeerConnectionClosed();
        void OnPeerConnectionError(string description);

        IVideoCapturer CreateVideoCapturer(IPeerConnectionFactory factory, IVideoSource videoSource);

        void OnConnected();
        void OnDisconnected();

        void OnLocalDescription(SessionDescription sdp);

        void OnIceCandidate(IceCandidate candidate);
        void OnIceCandidateRemoved(IceCandidate[] candidates);
        void OnIceConnected();
        void OnIceDisconnected();
    }
}
