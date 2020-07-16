// onotseike@hotmail.comPaula Aliu
using Android.Content;
using Org.Webrtc;
using WebRTC.Interfaces;
using ICameraVideoCapturer = Org.Webrtc.ICameraVideoCapturer;

namespace WebRTC.Android
{
    internal class CameraVideoCapturerNative : VideoCapturerNative, Interfaces.ICameraVideoCapturer
    {
        private readonly ICameraVideoCapturer _videoCapturer;

        public CameraVideoCapturerNative(ICameraVideoCapturer videoCapturer, Context context, VideoSource videoSource,
        IEglBaseContext eglBaseContext) : base(videoCapturer, context, videoSource, eglBaseContext)
        {
            _videoCapturer = videoCapturer;
        }

        public void SwitchCamera()
        {
            _videoCapturer.SwitchCamera(null);
        }
    }
}
