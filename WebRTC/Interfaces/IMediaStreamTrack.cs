// onotseike@hotmail.comPaula Aliu
using WebRTC.Enums;

namespace WebRTC.Interfaces
{
    public interface IMediaStreamTrack : INativeObject
    {
        string Kind { get; }
        string Label { get; }
        bool IsEnabled { get; set; }

        MediaStreamTrackState State { get; }
    }
}