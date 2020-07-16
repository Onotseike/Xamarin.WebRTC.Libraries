// onotseike@hotmail.comPaula Aliu
using WebRTC.Enums;

namespace WebRTC.Interfaces
{
    public interface IMediaSource : INativeObject
    {
        SourceState State { get; }
    }
}