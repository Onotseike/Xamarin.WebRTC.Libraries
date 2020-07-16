// onotseike@hotmail.comPaula Aliu
using WebRTC.Enums;

namespace WebRTC.Interfaces
{
    public interface IRtpTransceiver : INativeObject
    {
        RtpMediaType MediaType { get; }
        string Mid { get; }
        bool IsStopped { get; }
        RtpTransceiverDirection Direction { get; }
        IRtpSender Sender { get; }
        IRtpReceiver Receiver { get; }
        void Stop();
    }
}