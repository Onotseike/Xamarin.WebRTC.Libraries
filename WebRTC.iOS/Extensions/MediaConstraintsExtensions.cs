// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Classes;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class MediaConstraintsExtensions
    {
        public static RTCMediaConstraints ToNative(this MediaConstraints self)
        {
            return new RTCMediaConstraints(self.Mandatory.ToNative(), self.Optional.ToNative());
        }
    }
}
