// onotseike@hotmail.comPaula Aliu

using System;
using Org.Webrtc;
using WebRTC.Enums;
using SessionDescription = Org.Webrtc.SessionDescription;
namespace WebRTC.Android.Extensions
{
    internal static class EnumExtensions
    {
        public static PeerConnection.IceTransportsType ToNative(this IceTransportPolicy self)
        {
            switch (self)
            {
                case IceTransportPolicy.None:
                    return PeerConnection.IceTransportsType.None;
                case IceTransportPolicy.Relay:
                    return PeerConnection.IceTransportsType.Relay;
                case IceTransportPolicy.NoHost:
                    return PeerConnection.IceTransportsType.Nohost;
                case IceTransportPolicy.All:
                    return PeerConnection.IceTransportsType.All;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.BundlePolicy ToNative(this BundlePolicy self)
        {
            switch (self)
            {
                case BundlePolicy.Balanced:
                    return PeerConnection.BundlePolicy.Balanced;
                case BundlePolicy.MaxCompat:
                    return PeerConnection.BundlePolicy.Maxcompat;
                case BundlePolicy.MaxBundle:
                    return PeerConnection.BundlePolicy.Maxbundle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.RtcpMuxPolicy ToNative(this RtcpMuxPolicy self)
        {
            switch (self)
            {
                case RtcpMuxPolicy.Negotiate:
                    return PeerConnection.RtcpMuxPolicy.Negotiate;
                case RtcpMuxPolicy.Require:
                    return PeerConnection.RtcpMuxPolicy.Require;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.TcpCandidatePolicy ToNative(this TcpCandidatePolicy self)
        {
            switch (self)
            {
                case TcpCandidatePolicy.Enabled:
                    return PeerConnection.TcpCandidatePolicy.Enabled;
                case TcpCandidatePolicy.Disabled:
                    return PeerConnection.TcpCandidatePolicy.Disabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.CandidateNetworkPolicy ToNative(this CandidateNetworkPolicy self)
        {
            switch (self)
            {
                case CandidateNetworkPolicy.All:
                    return PeerConnection.CandidateNetworkPolicy.All;
                case CandidateNetworkPolicy.LowCost:
                    return PeerConnection.CandidateNetworkPolicy.LowCost;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.ContinualGatheringPolicy ToNative(this ContinualGatheringPolicy self)
        {
            switch (self)
            {
                case ContinualGatheringPolicy.Once:
                    return PeerConnection.ContinualGatheringPolicy.GatherOnce;
                case ContinualGatheringPolicy.Continually:
                    return PeerConnection.ContinualGatheringPolicy.GatherContinually;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.KeyType ToNative(this EncryptionKeyType self)
        {
            switch (self)
            {
                case EncryptionKeyType.Rsa:
                    return PeerConnection.KeyType.Rsa;
                case EncryptionKeyType.Ecdsa:
                    return PeerConnection.KeyType.Ecdsa;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.SdpSemantics ToNative(this SdpSemantics self)
        {
            switch (self)
            {
                case SdpSemantics.PlanB:
                    return PeerConnection.SdpSemantics.PlanB;
                case SdpSemantics.UnifiedPlan:
                    return PeerConnection.SdpSemantics.UnifiedPlan;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.TlsCertPolicy ToNative(this TlsCertPolicy self)
        {
            switch (self)
            {
                case TlsCertPolicy.Secure:
                    return PeerConnection.TlsCertPolicy.TlsCertPolicySecure;
                case TlsCertPolicy.InsecureNoCheck:
                    return PeerConnection.TlsCertPolicy.TlsCertPolicyInsecureNoCheck;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static SessionDescription.Type ToNative(this SdpType self)
        {
            switch (self)
            {
                case SdpType.Answer:
                    return SessionDescription.Type.Answer;
                case SdpType.Offer:
                    return SessionDescription.Type.Offer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static PeerConnection.PeerConnectionState ToNative(this PeerConnectionState self)
        {
            switch (self)
            {
                case PeerConnectionState.New:
                    return PeerConnection.PeerConnectionState.New;
                case PeerConnectionState.Connecting:
                    return PeerConnection.PeerConnectionState.Connecting;
                case PeerConnectionState.Connected:
                    return PeerConnection.PeerConnectionState.Connected;
                case PeerConnectionState.Disconnected:
                    return PeerConnection.PeerConnectionState.Disconnected;
                case PeerConnectionState.Failed:
                    return PeerConnection.PeerConnectionState.Failed;
                case PeerConnectionState.Closed:
                    return PeerConnection.PeerConnectionState.Closed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }

        public static SdpType ToNet(this SessionDescription.Type self)
        {
            if (self == SessionDescription.Type.Answer)
                return SdpType.Answer;
            if (self == SessionDescription.Type.Offer)
                return SdpType.Offer;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static MediaStreamTrackState ToNet(this MediaStreamTrack.State self)
        {
            if (self == MediaStreamTrack.State.Live)
                return MediaStreamTrackState.Live;
            if (self == MediaStreamTrack.State.Ended)
                return MediaStreamTrackState.Ended;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static IceConnectionState ToNet(this PeerConnection.IceConnectionState self)
        {
            if (self == PeerConnection.IceConnectionState.Checking)
                return IceConnectionState.Checking;
            if (self == PeerConnection.IceConnectionState.Closed)
                return IceConnectionState.Closed;
            if (self == PeerConnection.IceConnectionState.Completed)
                return IceConnectionState.Completed;
            if (self == PeerConnection.IceConnectionState.Connected)
                return IceConnectionState.Connected;
            if (self == PeerConnection.IceConnectionState.Disconnected)
                return IceConnectionState.Disconnected;
            if (self == PeerConnection.IceConnectionState.Failed)
                return IceConnectionState.Failed;
            if (self == PeerConnection.IceConnectionState.New)
                return IceConnectionState.New;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static IceGatheringState ToNet(this PeerConnection.IceGatheringState self)
        {
            if (self == PeerConnection.IceGatheringState.Complete)
                return IceGatheringState.Complete;
            if (self == PeerConnection.IceGatheringState.Gathering)
                return IceGatheringState.Gathering;
            if (self == PeerConnection.IceGatheringState.New)
                return IceGatheringState.New;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static SignalingState ToNet(this PeerConnection.SignalingState self)
        {
            if (self == PeerConnection.SignalingState.Closed)
                return SignalingState.Closed;
            if (self == PeerConnection.SignalingState.Stable)
                return SignalingState.Stable;
            if (self == PeerConnection.SignalingState.HaveLocalOffer)
                return SignalingState.HaveLocalOffer;
            if (self == PeerConnection.SignalingState.HaveLocalPranswer)
                return SignalingState.HaveLocalPrAnswer;
            if (self == PeerConnection.SignalingState.HaveRemoteOffer)
                return SignalingState.HaveRemoteOffer;
            if (self == PeerConnection.SignalingState.HaveRemotePranswer)
                return SignalingState.HaveRemotePrAnswer;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static DataChannelState ToNet(this DataChannel.State self)
        {
            if (self == DataChannel.State.Closed)
                return DataChannelState.Closed;
            if (self == DataChannel.State.Closing)
                return DataChannelState.Closing;
            if (self == DataChannel.State.Connecting)
                return DataChannelState.Connecting;
            if (self == DataChannel.State.Open)
                return DataChannelState.Open;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static PeerConnectionState ToNet(this PeerConnection.PeerConnectionState self)
        {
            if (self == PeerConnection.PeerConnectionState.Closed)
                return PeerConnectionState.Closed;
            if (self == PeerConnection.PeerConnectionState.Connected)
                return PeerConnectionState.Connected;
            if (self == PeerConnection.PeerConnectionState.Connecting)
                return PeerConnectionState.Connecting;
            if (self == PeerConnection.PeerConnectionState.Disconnected)
                return PeerConnectionState.Disconnected;
            if (self == PeerConnection.PeerConnectionState.Failed)
                return PeerConnectionState.Failed;
            if (self == PeerConnection.PeerConnectionState.New)
                return PeerConnectionState.New;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static SourceState ToNet(this MediaSource.State self)
        {
            if (self == MediaSource.State.Ended)
                return SourceState.Ended;
            if (self == MediaSource.State.Initializing)
                return SourceState.Initializing;
            if (self == MediaSource.State.Live)
                return SourceState.Live;
            if (self == MediaSource.State.Muted)
                return SourceState.Muted;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static RtpMediaType ToNet(this MediaStreamTrack.MediaType self)
        {
            if (self == MediaStreamTrack.MediaType.MediaTypeAudio)
                return RtpMediaType.Audio;
            if (self == MediaStreamTrack.MediaType.MediaTypeVideo)
                return RtpMediaType.Video;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }

        public static RtpTransceiverDirection ToNet(this RtpTransceiver.RtpTransceiverDirection self)
        {
            if (self == RtpTransceiver.RtpTransceiverDirection.Inactive)
                return RtpTransceiverDirection.Inactive;
            if (self == RtpTransceiver.RtpTransceiverDirection.RecvOnly)
                return RtpTransceiverDirection.RecvOnly;
            if (self == RtpTransceiver.RtpTransceiverDirection.SendOnly)
                return RtpTransceiverDirection.SendOnly;
            if (self == RtpTransceiver.RtpTransceiverDirection.SendRecv)
                return RtpTransceiverDirection.SendRecv;
            throw new ArgumentOutOfRangeException(nameof(self), self, null);
        }
    }
}
