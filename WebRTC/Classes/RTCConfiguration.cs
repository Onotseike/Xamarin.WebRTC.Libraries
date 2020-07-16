// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Enums;

namespace WebRTC.Classes
{
    public class RTCConfiguration
    {
        public RTCConfiguration(IceServer[] iceServers)
        {
            IceServers = iceServers;
            IceTransportPolicy = IceTransportPolicy.All;
            BundlePolicy = BundlePolicy.Balanced;
            RtcpMuxPolicy = RtcpMuxPolicy.Require;
            TcpCandidatePolicy = TcpCandidatePolicy.Enabled;
            CandidateNetworkPolicy = CandidateNetworkPolicy.All;
            AudioJitterBufferMaxPackets = 50;
            AudioJitterBufferFastAccelerate = false;
            IceConnectionReceivingTimeout = -1;
            IceBackupCandidatePairPingInterval = -1;
            KeyType = EncryptionKeyType.Ecdsa;
            ContinualGatheringPolicy = ContinualGatheringPolicy.Once;
            IceCandidatePoolSize = 0;
            ShouldPruneTurnPorts = false;
            ShouldPresumeWritableWhenFullyRelayed = false;
            IceCheckMinInterval = null;
            DisableIPv6OnWiFi = false;
            MaxIPv6Networks = 5;
            DisableIPv6 = false;
            SdpSemantics = SdpSemantics.PlanB;
            ActiveResetSrtpParams = false;
            UseMediaTransport = false;
            UseMediaTransportForDataChannels = false;
            EnableDtlsSrtp = true;
        }

        public IceServer[] IceServers { get; }

        public IceTransportPolicy IceTransportPolicy { get; set; }

        public BundlePolicy BundlePolicy { get; set; }

        public RtcpMuxPolicy RtcpMuxPolicy { get; set; }

        public TcpCandidatePolicy TcpCandidatePolicy { get; set; }

        public CandidateNetworkPolicy CandidateNetworkPolicy { get; set; }

        public ContinualGatheringPolicy ContinualGatheringPolicy { get; set; }



        public bool DisableIPv6 { get; set; }

        public bool DisableIPv6OnWiFi { get; set; }

        public int MaxIPv6Networks { get; set; }

        //public bool DisableLinkLocalNetworks { get; set; }

        public int AudioJitterBufferMaxPackets { get; set; }

        public bool AudioJitterBufferFastAccelerate { get; set; }

        public int IceConnectionReceivingTimeout { get; set; }

        public int IceBackupCandidatePairPingInterval { get; set; }

        public EncryptionKeyType KeyType { get; set; }

        public int IceCandidatePoolSize { get; set; }

        public bool ShouldPruneTurnPorts { get; set; }

        //public TurnPortPrunePolicy TurnPortPrunePolicy {get;set;}

        public bool ShouldPresumeWritableWhenFullyRelayed { get; set; }

        public int? IceCheckMinInterval { get; set; }

        public SdpSemantics SdpSemantics { get; set; }

        public bool ActiveResetSrtpParams { get; set; }
        public bool UseMediaTransport { get; set; }
        public bool UseMediaTransportForDataChannels { get; set; }

        public bool EnableDtlsSrtp { get; set; }

        public RTCCertificate Certificate { get; set; }

        //CryptoOptions CryptoOptions { get; set; }

        // public int RtcpAudioReportIntervalMs { get; set; }
        // public int RtcpVideoReportIntervalMs { get; set; }
    }
}
