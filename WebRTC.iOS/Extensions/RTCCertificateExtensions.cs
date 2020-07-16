// onotseike@hotmail.comPaula Aliu
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class RTCCertificateExtensions
    {
        public static RTCCertificate ToNative(this Classes.RTCCertificate self) =>
            new RTCCertificate(self.PrivateKey, self.Certificate);

        public static Classes.RTCCertificate ToNet(this RTCCertificate self) =>

            new Classes.RTCCertificate(self.Private_key, self.Certificate);
    }
}
