// onotseike@hotmail.comPaula Aliu
namespace WebRTC.Interfaces
{
    public interface IRtpSender : INativeObject
    {
        string SenderId { get; }

        IMediaStreamTrack Track { get; }
    }
}