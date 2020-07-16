// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Classes;

namespace WebRTC.Android.Extensions
{
    internal static class RTCCertificateExtension
    {
        public static RtcCertificatePem ToNative(this RTCCertificate self) =>
            new RtcCertificatePem(self.PrivateKey, self.Certificate);

        public static RTCCertificate ToNet(this RtcCertificatePem self) =>
            new RTCCertificate(self.PrivateKey, self.Certificate);
    }
}
