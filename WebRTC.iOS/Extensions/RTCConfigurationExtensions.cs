// onotseike@hotmail.comPaula Aliu

using System.Linq;
using Foundation;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class RTCConfigurationExtensions
    {
        public static RTCConfiguration ToNative(this Classes.RTCConfiguration self)
        {
            return new RTCConfiguration
            {
                IceServers = self.IceServers.ToNative().ToArray(),
                IceTransportPolicy = self.IceTransportPolicy.ToNative(),
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
                ShouldPruneTurnPorts = self.ShouldPruneTurnPorts,
                ShouldPresumeWritableWhenFullyRelayed = self.ShouldPresumeWritableWhenFullyRelayed,
                IceCheckMinInterval = self.IceCheckMinInterval.HasValue
                    ? new NSNumber(self.IceCheckMinInterval.Value)
                    : null,
                DisableIPV6OnWiFi = self.DisableIPv6OnWiFi,
                MaxIPv6Networks = self.MaxIPv6Networks,
                DisableIPV6 = self.DisableIPv6,
                SdpSemantics = self.SdpSemantics.ToNative(),
                ActiveResetSrtpParams = self.ActiveResetSrtpParams,
                UseMediaTransport = self.UseMediaTransport,
                UseMediaTransportForDataChannels = self.UseMediaTransportForDataChannels,
                //EnableDtlsSrtp = self.EnableDtlsSrtp.HasValue ? new Boolean(self.EnableDtlsSrtp.Value) : null,
                Certificate = self.Certificate?.ToNative()
            };
        }
    }
}
