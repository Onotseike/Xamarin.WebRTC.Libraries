// onotseike@hotmail.comPaula Aliu
using WebRTC.Enums;

namespace WebRTC.Interfaces
{
    public interface IVideoCapturer : INativeObject
    {
        bool IsScreencast { get; }

        void StartCapture();

        void StartCapture(int videoWidth, int videoHeight, int fps);
        void StopCapture();
    }
}