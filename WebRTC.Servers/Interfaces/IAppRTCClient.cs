// onotseike@hotmail.comPaula Aliu
using System;
using System.Data;

using WebRTC.Classes;

namespace WebRTC.Servers.Interfaces
{
    public interface IAppRTCClient<in TConnectionParam> where TConnectionParam : IConnectionParameters
    {
        ConnectionState State { get; }

        void Connect(TConnectionParam connectionParam);
        void Disconnect();

        void SendSdpOffer(SessionDescription sdp);
        void SendSdpAnswer(SessionDescription sdp);

        void SendLocalIceCandidate(IceCandidate candidate);
        void SendLocalIceCandidateRemovals(IceCandidate[] candidates);
    }
}
