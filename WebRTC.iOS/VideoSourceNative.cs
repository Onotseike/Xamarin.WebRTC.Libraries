// onotseike@hotmail.comPaula Aliu
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class VideoSourceNative : MediaSourceNative, IVideoSource
    {
        private readonly RTCVideoSource _videoSource;

        public VideoSourceNative(RTCVideoSource videoSource) : base(videoSource)
        {
            _videoSource = videoSource;
        }

        public void AdaptOutputFormat(int width, int height, int fps)
        {
            _videoSource.AdaptOutputFormatToWidth(width, height, fps);
        }
    }
}