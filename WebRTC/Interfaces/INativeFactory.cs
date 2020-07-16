// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Classes;
using WebRTC.Enums;

namespace WebRTC.Interfaces
{
    public interface INativeFactory
    {
        IPeerConnectionFactory CreatePeerConnectionFactory();
        RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires);

        void StopInternalTracingCapture();
        void ShutdownInternalTracer();
    }
}
