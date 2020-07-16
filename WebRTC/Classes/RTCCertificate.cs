// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Enums;

namespace WebRTC.Classes
{
    public class RTCCertificate
    {
        private const long DefaultExpiry = 2592000L;

        public RTCCertificate(string privateKey, string certificate)
        {
            PrivateKey = privateKey;
            Certificate = certificate;
        }

        public string PrivateKey { get; }

        public string Certificate { get; }

        public static RTCCertificate GenerateCertificate() =>
            GenerateCertificate(EncryptionKeyType.Ecdsa, DefaultExpiry);

        public static RTCCertificate GenerateCertificate(EncryptionKeyType keyType) =>
            GenerateCertificate(keyType, DefaultExpiry);

        public static RTCCertificate GenerateCertificate(EncryptionKeyType keyType, long expires) =>
            NativeFactory.GenerateCertificate(keyType, expires);
    }
}
