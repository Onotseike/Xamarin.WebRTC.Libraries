// onotseike@hotmail.comPaula Aliu
namespace WebRTC.Interfaces
{
    public interface IRtpReceiver : INativeObject
    {
        string Id { get; }

        IMediaStreamTrack Track { get; }
    }
}