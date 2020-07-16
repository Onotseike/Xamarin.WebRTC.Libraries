// onotseike@hotmail.comPaula Aliu
using System;
using System.Linq;
using Java.Lang;
using Org.Webrtc;
using WebRTC.Classes;

namespace WebRTC.Android.Extensions
{
    internal static class RTCConfigurationExtensions
    {
        public static PeerConnection.RTCConfiguration ToNative(this RTCConfiguration self)
        {
            return new PeerConnection.RTCConfiguration(self.IceServers.ToNative().ToList())
            {
                IceTransportsType = self.IceTransportPolicy.ToNative(),
                BundlePolicy = self.BundlePolicy.ToNative(),
                RtcpMuxPolicy = self.RtcpMuxPolicy.ToNative(),
                TcpCandidatePolicy = self.TcpCandidatePolicy.ToNative(),
                CandidateNetworkPolicy = self.CandidateNetworkPolicy.ToNative(),
                AudioJitterBufferMaxPackets = self.AudioJitterBufferMaxPackets,
                AudioJitterBufferFastAccelerate = self.AudioJitterBufferFastAccelerate,
                IceConnectionReceivingTimeout = self.IceConnectionReceivingTimeout,
                KeyType = self.KeyType.ToNative(),
                ContinualGatheringPolicy = self.ContinualGatheringPolicy.ToNative(),
                IceCandidatePoolSize = self.IceCandidatePoolSize,
                PruneTurnPorts = self.ShouldPruneTurnPorts,
                PresumeWritableWhenFullyRelayed = self.ShouldPresumeWritableWhenFullyRelayed,
                IceCheckMinInterval = self.IceCheckMinInterval.HasValue
                    ? new Integer(self.IceCheckMinInterval.Value)
                    : null,
                DisableIPv6OnWifi = self.DisableIPv6OnWiFi,
                MaxIPv6Networks = self.MaxIPv6Networks,
                DisableIpv6 = self.DisableIPv6,
                SdpSemantics = self.SdpSemantics.ToNative(),
                ActiveResetSrtpParams = self.ActiveResetSrtpParams,
                UseMediaTransport = self.UseMediaTransport,
                UseMediaTransportForDataChannels = self.UseMediaTransportForDataChannels,
                EnableDtlsSrtp = !self.EnableDtlsSrtp ? new Java.Lang.Boolean(true) : null,
                Certificate = self.Certificate?.ToNative()
            };
        }
    }
}
