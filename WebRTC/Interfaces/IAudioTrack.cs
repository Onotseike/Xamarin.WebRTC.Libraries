// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Interfaces
{
    public interface IAudioTrack : IMediaStreamTrack
    {
        float Volume { get; set; }
    }
}
