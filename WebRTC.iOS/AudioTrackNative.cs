// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class AudioTrackNative : MediaStreamTrackNative, IAudioTrack
    {
        private readonly RTCAudioTrack _audioTrack;
        public AudioTrackNative(RTCAudioTrack audioTrack) : base(audioTrack)
        {
            _audioTrack = audioTrack;
        }

        public float Volume
        {
            get => (float)_audioTrack.Source.Volume;
            set => _audioTrack.Source.Volume = value;
        }
    }
}
