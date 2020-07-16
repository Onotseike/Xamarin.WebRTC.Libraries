// onotseike@hotmail.comPaula Aliu
using System;
using Foundation;
using WebRTC.Enums;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal class NativeFactory : INativeFactory
    {

        public IPeerConnectionFactory CreatePeerConnectionFactory() => new PeerConnectionFactoryNative();

        public Classes.RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires)
        {
            return RTCCertificate.GenerateCertificateWithParams(new NSDictionary<NSString, NSObject>(
                new[] { "expires".ToNative(), "name".ToNative() },
                new NSObject[] { new NSNumber(expires), keyType.ToStringNative() }
            )).ToNet();
        }

        public void ShutdownInternalTracer()
        {
            RTCTracing.RTCShutdownInternalTracer();
        }

        public void StopInternalTracingCapture()
        {
            RTCTracing.RTCStopInternalCapture();
        }
    }
}
