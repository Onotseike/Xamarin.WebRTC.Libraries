// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    internal class AudioSourceNative : MediaSourceNative, IAudioSource
    {
        public AudioSourceNative(RTCAudioSource audioSource) : base(audioSource)
        {
        }
    }
}
