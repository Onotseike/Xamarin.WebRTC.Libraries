// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Interfaces;
using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns
{
    public interface IRTCEngineEvents
    {
        void OnPeerFactoryCreated(IPeerConnectionFactory peerConnectionFactory);

        void OnDisconnect(DisconnectType disconnectType);

        IVideoCapturer CreateVideoCapturer(IPeerConnectionFactory factory, IVideoSource videoSource);

        void ReadyToStart();

        void OnError(string description);
    }
}
