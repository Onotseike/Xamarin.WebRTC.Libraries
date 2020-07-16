// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class MediaSourceNative : NativeObjectBase, IMediaSource
    {
        private readonly MediaSource _mediaSource;

        public MediaSourceNative(MediaSource mediaSource) : base(mediaSource)
        {
            _mediaSource = mediaSource;
        }

        public SourceState State => _mediaSource.InvokeState().ToNet();
    }
}