// onotseike@hotmail.comPaula Aliu
using System.Collections.Generic;
using System.Linq;
using WebRTC.Classes;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class IceCandidateExtension
    {
        public static RTCIceCandidate ToNative(this IceCandidate self)
        {
            return new RTCIceCandidate(self.SdpMid, self.SdpMLineIndex, self.Sdp);
        }

        public static IEnumerable<RTCIceCandidate> ToNative(this IEnumerable<IceCandidate> self)
        {
            return self.Select(ToNative);
        }

        public static IceCandidate ToNet(this RTCIceCandidate self)
        {
            return new IceCandidate(self.Sdp, self.SdpMid, self.SdpMLineIndex);
        }

        public static IEnumerable<IceCandidate> ToNet(this RTCIceCandidate[] self)
        {
            return self.Select(ToNet);
        }
    }
}
