// onotseike@hotmail.comPaula Aliu


using WebRTC.Classes;
using WebRTC.Servers.Enums;

namespace WebRTC.Servers.Interfaces
{
    public interface IRTCClient<in TConnectionParam> where TConnectionParam : IConnectionParameters
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
