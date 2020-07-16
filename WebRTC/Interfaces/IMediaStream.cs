// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Interfaces
{
    public interface IMediaStream : INativeObject
    {
        string StreamId { get; }
        IAudioTrack[] AudioTracks { get; }
        IVideoTrack[] VideoTracks { get; }

        void AddTrack(IAudioTrack audioTrack);
        void AddTrack(IVideoTrack videoTrack);

        void RemoveTrack(IAudioTrack audioTrack);
        void RemoveTrack(IVideoTrack videoTrack);
    }
}
