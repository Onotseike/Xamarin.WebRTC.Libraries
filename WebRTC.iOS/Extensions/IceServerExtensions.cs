// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;
using WebRTC.Classes;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class IceServerExtensions
    {
        public static RTCIceServer ToNative(this IceServer self)
        {
            return new RTCIceServer(self.Urls, self.Username, self.Password, self.TlsCertPolicy.ToNative());
        }

        public static IEnumerable<RTCIceServer> ToNative(this IEnumerable<IceServer> self)
        {
            return self.Select(ToNative);
        }
    }
}
