// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal abstract class MediaStreamTrackNative : NativeObjectBase, IMediaStreamTrack
    {
        private readonly RTCMediaStreamTrack _track;

        protected MediaStreamTrackNative(RTCMediaStreamTrack track) : base(track)
        {
            _track = track;
        }

        public string Kind => _track.Kind;
        public string Label => _track.TrackId;
        
        public bool IsEnabled
        {
            get => _track.IsEnabled;
            set => _track.IsEnabled = value;
        }


        public MediaStreamTrackState State => _track.ReadyState.ToNet();
    }
}