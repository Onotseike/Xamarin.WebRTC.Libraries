// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;
using Org.Webrtc;
using WebRTC.Classes;

namespace WebRTC.Android.Extensions
{
    internal static class IceServerExtensions
    {
        public static PeerConnection.IceServer ToNative(this IceServer self)
        {
            var builder = PeerConnection.IceServer.InvokeBuilder(self.Urls)
                .SetTlsCertPolicy(self.TlsCertPolicy.ToNative());

            if (!string.IsNullOrEmpty(self.Username))
                builder.SetUsername(self.Username)
                    .SetPassword(self.Password);

            return builder.CreateIceServer();
        }

        public static IEnumerable<PeerConnection.IceServer> ToNative(this IEnumerable<IceServer> self)
        {
            return self.Select(ToNative);
        }
    }
}
