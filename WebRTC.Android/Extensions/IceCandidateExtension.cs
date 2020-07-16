// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;

namespace WebRTC.Android.Extensions
{
    internal static class IceCandidateExtension
    {
        public static IceCandidate ToNative(this Classes.IceCandidate self)
        {
            return new IceCandidate(self.SdpMid, self.SdpMLineIndex, self.Sdp);
        }

        public static Classes.IceCandidate ToNet(this IceCandidate self)
        {
            return new Classes.IceCandidate(self.Sdp, self.SdpMid, self.SdpMLineIndex);
        }
    }
}
