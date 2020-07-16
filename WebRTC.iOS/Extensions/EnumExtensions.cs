// onotseike@hotmail.comPaula Aliu
using System;
using Foundation;
using WebRTC.Enums;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class EnumExtensions
    {
        public static RTCIceTransportPolicy ToNative(this IceTransportPolicy self)
        {
            return (RTCIceTransportPolicy)self;
        }

        public static RTCBundlePolicy ToNative(this BundlePolicy self)
        {
            return (RTCBundlePolicy)self;
        }

        public static RTCRtcpMuxPolicy ToNative(this RtcpMuxPolicy self)
        {
            return (RTCRtcpMuxPolicy)self;
        }

        public static RTCTcpCandidatePolicy ToNative(this TcpCandidatePolicy self)
        {
            return (RTCTcpCandidatePolicy)self;
        }

        public static RTCCandidateNetworkPolicy ToNative(this CandidateNetworkPolicy self)
        {
            return (RTCCandidateNetworkPolicy)self;
        }

        public static RTCContinualGatheringPolicy ToNative(this ContinualGatheringPolicy self)
        {
            return (RTCContinualGatheringPolicy)self;
        }

        public static RTCEncryptionKeyType ToNative(this EncryptionKeyType self)
        {
            return (RTCEncryptionKeyType)self;
        }

        public static NSString ToStringNative(this EncryptionKeyType self)
        {
            switch (self)
            {
                case EncryptionKeyType.Rsa:
                    return new NSString("RSASSA-PKCS1-v1_5");
                case EncryptionKeyType.Ecdsa:
                    return new NSString("ECDSA");
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static RTCSdpSemantics ToNative(this SdpSemantics self)
        {
            return (RTCSdpSemantics)self;
        }

        public static RTCTlsCertPolicy ToNative(this TlsCertPolicy self)
        {
            return (RTCTlsCertPolicy)self;
        }

        public static RTCSdpType ToNative(this SdpType self)
        {
            return (RTCSdpType)self;
        }

        public static RTCPeerConnectionState ToNative(this PeerConnectionState self)
        {
            return (RTCPeerConnectionState)self;
        }

        public static SdpType ToNet(this RTCSdpType self)
        {
            return (SdpType)self;
        }

        public static MediaStreamTrackState ToNet(this RTCMediaStreamTrackState self)
        {
            return (MediaStreamTrackState)self;
        }

        public static IceConnectionState ToNet(this RTCIceConnectionState self)
        {
            return (IceConnectionState)self;
        }

        public static IceGatheringState ToNet(this RTCIceGatheringState self)
        {
            return (IceGatheringState)self;
        }

        public static SignalingState ToNet(this RTCSignalingState self)
        {
            return (SignalingState)self;
        }

        public static DataChannelState ToNet(this RTCDataChannelState self)
        {
            return (DataChannelState)self;
        }

        public static PeerConnectionState ToNet(this RTCPeerConnectionState self)
        {
            return (PeerConnectionState)self;
        }

        public static SourceState ToNet(this SourceState self)
        {
            return self;
        }

        public static RtpMediaType ToNet(this RTCRtpMediaType self)
        {
            return (RtpMediaType)self;
        }

        public static RtpTransceiverDirection ToNet(this RTCRtpTransceiverDirection self)
        {
            return (RtpTransceiverDirection)self;
        }

        public static SourceState ToNet(this RTCSourceState self)
        {
            return (SourceState)self;
        }
    }
}
