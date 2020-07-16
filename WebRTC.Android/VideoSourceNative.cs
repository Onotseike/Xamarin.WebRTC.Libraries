// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class VideoSourceNative : MediaSourceNative, IVideoSource
    {
        private readonly VideoSource _videoSource;

        public VideoSourceNative(VideoSource videoSource) : base(videoSource)
        {
            _videoSource = videoSource;
        }

        public void AdaptOutputFormat(int width, int height, int fps)
        {
            _videoSource.AdaptOutputFormat(width, height, fps);
        }
    }
}
