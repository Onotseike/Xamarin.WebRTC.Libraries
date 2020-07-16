// onotseike@hotmail.comPaula Aliu
using System;
using System.Linq;
using Org.Webrtc;

namespace WebRTC.Android.Extensions
{
    internal static class MediaConstraintsExtensions
    {
        public static MediaConstraints ToNative(this Classes.MediaConstraints self)
        {
            var optionals = self.Optional.Select(p => new MediaConstraints.KeyValuePair(p.Key, p.Value)).ToList();
            var mandatory = self.Mandatory.Select(p => new MediaConstraints.KeyValuePair(p.Key, p.Value)).ToList();
            return new MediaConstraints
            {
                Mandatory = mandatory,
                Optional = optionals
            };
        }
    }
}
