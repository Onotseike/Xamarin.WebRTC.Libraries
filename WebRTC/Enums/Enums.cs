// onotseike@hotmail.com
//Paula Aliu
using System.Runtime.Serialization;
namespace WebRTC.Enums
{
    /// <summary>
    /// SourceState 
    /// </summary>
    public enum SourceState : long
    {
        Initializing,
        Live,
        Ended,
        Muted
    }

    public enum MediaStreamTrackState : long
    {
        Live,
        Ended
    }

    public enum IceTransportPolicy : long
    {
        None,
        Relay,
        NoHost,
        All
    }

    public enum BundlePolicy : long
    {
        Balanced,
        MaxCompat,
        MaxBundle
    }

    public enum RtcpMuxPolicy : long
    {
        Negotiate,
        Require
    }

    public enum TcpCandidatePolicy : long
    {
        Enabled,
        Disabled
    }

    public enum CandidateNetworkPolicy : long
    {
        All,
        LowCost
    }

    public enum ContinualGatheringPolicy : long
    {
        Once,
        Continually
    }

    public enum EncryptionKeyType : long
    {
        Rsa,
        Ecdsa
    }

    public enum SdpSemantics : long
    {
        PlanB,
        UnifiedPlan
    }

    public enum DataChannelState : long
    {
        Connecting,
        Open,
        Closing,
        Closed
    }

    public enum TlsCertPolicy : ulong
    {
        Secure,
        InsecureNoCheck
    }

    public enum SignalingState : long
    {
        Stable,
        HaveLocalOffer,
        HaveLocalPrAnswer,
        HaveRemoteOffer,
        HaveRemotePrAnswer,
        Closed
    }

    public enum IceConnectionState : long
    {
        New,
        Checking,
        Connected,
        Completed,
        Failed,
        Disconnected,
        Closed,
        Count
    }
    
    public enum PeerConnectionState : long
    {
        New,
        Connecting,
        Connected,
        Disconnected,
        Failed,
        Closed
    }

    public enum IceGatheringState : long
    {
        New,
        Gathering,
        Complete
    }

    public enum StatsOutputLevel : long
    {
        Standard,
        Debug
    }
    
    public enum RtpMediaType : long
    {
        Audio,
        Video,
        Data
    }

    public enum RtpTransceiverDirection : long
    {
        SendRecv,
        SendOnly,
        RecvOnly,
        Inactive
    }

    public enum SdpType : long
    {
        [EnumMember(Value = "offer")]
        Offer,
        Answer,
        [EnumMember(Value = "answer")]
        PrAnswer
    }

    public enum ScalingType
    {
        AspectFit,
        AspectFill,
        AspectBalanced
    }

    public enum VideoCodec
    {
        VP8,
        VP9,
        H264High,
        H264
    }
}
