// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using static WebRTC.Classes.DataChannel;

namespace WebRTC.Android.Extensions
{
    internal static class DataChannelConfigurationExtension
    {
        public static DataChannel.Init ToNative(this DataChannelConfiguration self)
        {
            return new DataChannel.Init
            {
                Id = self.Id,
                Negotiated = self.IsNegotiated,
                MaxRetransmitTimeMs = self.MaxRetransmitTimeMs,
                MaxRetransmits = self.MaxRetransmits,
                Ordered = self.IsOrdered,
                Protocol = self.Protocol
            };
        }
    }
}
