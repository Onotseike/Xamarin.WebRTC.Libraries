// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;

namespace WebRTC.Interfaces
{
    public interface IPeerConnectionFactory : INativeObject
    {
        IPeerConnection CreatePeerConnection(RTCConfiguration configuration,
            IPeerConnectionListener peerConnectionListener);

        IAudioSource CreateAudioSource(MediaConstraints mediaConstraints);

        IAudioTrack CreateAudioTrack(string id, IAudioSource audioSource);

        IVideoSource CreateVideoSource(bool isScreencast);

        IVideoTrack CreateVideoTrack(string id, IVideoSource videoSource);

        ICameraVideoCapturer CreateCameraCapturer(IVideoSource videoSource, bool frontCamera);

        IFileVideoCapturer CreateFileCapturer(IVideoSource videoSource, string file);

        bool StartAecDump(string file, int fileSizeLimitBytes);
        void StopAecDump();
    }
}