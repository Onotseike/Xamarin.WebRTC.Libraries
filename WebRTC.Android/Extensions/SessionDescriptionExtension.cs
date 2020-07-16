// onotseike@hotmail.comPaula Aliu
using SessionDescription = Org.Webrtc.SessionDescription;

namespace WebRTC.Android.Extensions
{
    internal static class SessionDescriptionExtension
    {
        public static SessionDescription ToNative(this Classes.SessionDescription self)
        {
            return new SessionDescription(self.Type.ToNative(), self.Sdp);
        }

        public static Classes.SessionDescription ToNet(this SessionDescription self)
        {
            return new Classes.SessionDescription(self.SdpType.ToNet(), self.Description);
        }
    }
}
