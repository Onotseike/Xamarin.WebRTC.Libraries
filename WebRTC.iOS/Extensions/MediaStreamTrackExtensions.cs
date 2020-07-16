// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Extensions;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS.Extensions
{
    internal static class MediaStreamTrackExtensions
    {
        public static RTCMediaStreamTrack ToNative(this IMediaStreamTrack self)
        {
            return self.ToNative<RTCMediaStreamTrack>();
        }

        public static IMediaStreamTrack ToNet(this RTCMediaStreamTrack self)
        {
            switch (self.Kind)
            {
                case Constants.Constants.AudioTrackKind:
                    return new AudioTrackNative((RTCAudioTrack)self);
                case Constants.Constants.VideoTrackKind:
                    return new VideoTrackNative((RTCVideoTrack)self);
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}
