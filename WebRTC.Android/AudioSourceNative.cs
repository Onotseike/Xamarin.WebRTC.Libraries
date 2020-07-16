// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class AudioSourceNative : MediaSourceNative, IAudioSource
    {
        public AudioSourceNative(AudioSource audioSource) : base(audioSource)
        {
        }
    }
}
