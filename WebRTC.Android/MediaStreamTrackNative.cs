// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class MediaStreamTrackNative : NativeObjectBase, IMediaStreamTrack
    {
        private readonly MediaStreamTrack _mediaStreamTrack;
        public MediaStreamTrackNative(MediaStreamTrack mediaStreamTrack) : base(mediaStreamTrack)
        {
            _mediaStreamTrack = mediaStreamTrack;
        }

        public string Kind => _mediaStreamTrack.Kind();

        public string Label => _mediaStreamTrack.Id();

        public bool IsEnabled
        {
            get => _mediaStreamTrack.Enabled();
            set => _mediaStreamTrack.SetEnabled(value);
        }

        public MediaStreamTrackState State => _mediaStreamTrack.InvokeState().ToNet();
    }
}