// onotseike@hotmail.comPaula Aliu
using Android.Content;
using Org.Webrtc;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class FileVideoCapturerNative : VideoCapturerNative, IFileVideoCapturer
    {
        private readonly FileVideoCapturer _fileVideoCapturer;

        public FileVideoCapturerNative(FileVideoCapturer fileVideoCapturer, Context context, VideoSource videoSource,
            IEglBaseContext eglBaseContext) : base(fileVideoCapturer, context, videoSource, eglBaseContext)
        {
            _fileVideoCapturer = fileVideoCapturer;
        }
    }
}
