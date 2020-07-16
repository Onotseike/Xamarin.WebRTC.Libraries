// onotseike@hotmail.comPaula Aliu
using Android.Content;
using Org.Webrtc;
using WebRTC.Android.Extensions;
using PeerConnectionFactory = Org.Webrtc.PeerConnectionFactory;
using WebRTC.Interfaces;
using WebRTC.Classes;
using WebRTC.Enums;

namespace WebRTC.Android
{
    internal class NativeFactory : INativeFactory
    {
        private readonly Context _context;

        public NativeFactory(Context context)
        {
            _context = context;
        }

        public IPeerConnectionFactory CreatePeerConnectionFactory() => new PeerConnectionFactoryNative(_context);

        public RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires) => RtcCertificatePem.GenerateCertificate(keyType.ToNative(), expires).ToNet();

        public void StopInternalTracingCapture()
        {
            PeerConnectionFactory.StopInternalTracingCapture();
        }

        public void ShutdownInternalTracer()
        {
            PeerConnectionFactory.ShutdownInternalTracer();
        }
    }
}
