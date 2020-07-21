// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Classes;

namespace WebRTC.Servers.ServerFxns
{
    public class PeerConnectionParameters
    {
        public PeerConnectionParameters(IceServer[] iceServers)
        {
            IceServers = iceServers;
        }

        public IceServer[] IceServers { get; }

        public bool IsScreenCast { get; set; }
        public bool IsVideoCallEnabled { get; set; }
        public bool LoopBack { get; set; }
        public bool Tracing { get; set; }

        public int VideoHeight { get; set; }
        public int VideoWidth { get; set; }
        public int VideoFPS { get; set; }

        public bool NoAudioProcessing { get; set; }
        public bool AECDump { get; set; }
        public string AEcDumpFile { get; set; }

        public bool EnableRTCEventLog { get; set; } = false;

        public string RTCEventLogDirectory { get; set; }
    }
}
