// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Classes;

namespace WebRTC.Servers.Interfaces
{
    public interface ISignalingEvents<in TSignalParam> where TSignalParam : ISignalingParameters
    {
        void OnChannelConnected(TSignalParam signalParam);
        void OnChannelClose();
        void OnChannelError(string Description);
        void OnRemoteDescription(SessionDescription sdp);
        void OneRemoteIceCandidate(IceCandidate candidate);
        void OnRemoteIceCandidateRemoved(IceCandidate[] candidates);
    }
}
