// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Interfaces;

namespace WebRTC.Classes
{
    public class PeerConnectionFactory : IPeerConnectionFactory
    {
        private readonly IPeerConnectionFactory _factory;

        public PeerConnectionFactory()
        {
            _factory = NativeFactory.CreatePeerConnectionFactory();
        }
        public object NativeObject => _factory;
        public void Dispose()
        {
            _factory.Dispose();
        }

        public IPeerConnection CreatePeerConnection(RTCConfiguration configuration,
            IPeerConnectionListener peerConnectionListener)
        {
            return _factory.CreatePeerConnection(configuration, peerConnectionListener);
        }

        public IAudioSource CreateAudioSource(MediaConstraints mediaConstraints)
        {
            return _factory.CreateAudioSource(mediaConstraints);
        }

        public IAudioTrack CreateAudioTrack(string id, IAudioSource audioSource)
        {
            return _factory.CreateAudioTrack(id, audioSource);
        }

        public IVideoSource CreateVideoSource(bool isScreencast)
        {
            return _factory.CreateVideoSource(isScreencast);
        }

        public IVideoTrack CreateVideoTrack(string id, IVideoSource videoSource)
        {
            return _factory.CreateVideoTrack(id, videoSource);
        }

        public ICameraVideoCapturer CreateCameraCapturer(IVideoSource videoSource, bool frontCamera)
        {
            return _factory.CreateCameraCapturer(videoSource, frontCamera);
        }

        public IFileVideoCapturer CreateFileCapturer(IVideoSource videoSource, string file)
        {
            return _factory.CreateFileCapturer(videoSource, file);
        }

        public bool StartAecDump(string file, int fileSizeLimitBytes)
        {
            return _factory.StartAecDump(file, fileSizeLimitBytes);
        }

        public void StopAecDump()
        {
            _factory.StopAecDump();
        }

        public static void StopInternalTracingCapture() => NativeFactory.StopInternalTracingCapture();
        public static void ShutdownInternalTracer() => NativeFactory.ShutdownInternalTracer();
    }
}
