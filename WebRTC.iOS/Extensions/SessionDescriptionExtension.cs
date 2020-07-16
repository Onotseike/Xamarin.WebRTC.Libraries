// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class SessionDescriptionExtension
    {
        public static RTCSessionDescription ToNative(this Classes.SessionDescription self)
        {
            return new RTCSessionDescription(self.Type.ToNative(), self.Sdp);
        }

        public static SessionDescription ToNet(this RTCSessionDescription self)
        {
            return new SessionDescription(self.Type.ToNet(), self.Sdp);
        }
    }
}
