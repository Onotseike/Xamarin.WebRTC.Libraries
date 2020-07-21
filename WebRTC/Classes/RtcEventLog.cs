// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Interfaces;

namespace WebRTC.Classes
{
    public class RtcEventLog
    {
        private IPeerConnection peerConnection;
        private string file;
        private WebRTC.Servers.Interfaces.ILogger logger;

        public RtcEventLog(IPeerConnection peerConnection, string file, WebRTC.Servers.Interfaces.ILogger logger)
        {
            this.peerConnection = peerConnection;
            this.file = file;
            this.logger = logger;
        }
    }
}
