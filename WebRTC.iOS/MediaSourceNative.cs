// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal class MediaSourceNative : NativeObjectBase, IMediaSource
    {
        private readonly RTCMediaSource _mediaSource;

        protected MediaSourceNative(RTCMediaSource mediaSource) : base(mediaSource)
        {
            _mediaSource = mediaSource;
        }

        public SourceState State => _mediaSource.State.ToNet();
    }
}
