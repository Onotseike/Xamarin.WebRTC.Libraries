// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Extensions;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class VideoTrackNative : MediaStreamTrackNative, IVideoTrack
    {
        private readonly RTCVideoTrack _videoTrack;

        public VideoTrackNative(RTCVideoTrack videoTrack) : base(videoTrack)
        {
            _videoTrack = videoTrack;
        }

        public void AddRenderer(IVideoRenderer videoRenderer)
        {
            _videoTrack.AddRenderer(videoRenderer.ToNative<IRTCVideoRenderer>());
        }

        public void RemoveRenderer(IVideoRenderer videoRenderer)
        {
            _videoTrack.RemoveRenderer(videoRenderer.ToNative<IRTCVideoRenderer>());

        }
    }
}
