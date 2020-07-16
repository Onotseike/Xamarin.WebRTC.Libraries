// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Interfaces;
using WebRTC.Extensions;

namespace WebRTC.Android.Extensions
{
    internal static class MediaStreamTrackExtensions
    {
        public static MediaStreamTrack ToNative(this IMediaStreamTrack self)
        {
            return self.ToNative<MediaStreamTrack>();
        }

        public static IMediaStreamTrack ToNet(this MediaStreamTrack self)
        {
            switch (self.Kind())
            {
                case Constants.Constants.AudioTrackKind:
                    return new AudioTrackNative((AudioTrack)self);
                case Constants.Constants.VideoTrackKind:
                    return new VideoTrackNative((VideoTrack)self);
                default:
                    throw new ArgumentOutOfRangeException(nameof(self), self, null);
            }
        }
    }
}
