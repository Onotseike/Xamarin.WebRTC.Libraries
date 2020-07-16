// onotseike@hotmail.comPaula Aliu
using System.Diagnostics;
using WebRTC.Classes;
using WebRTC.Extensions;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;

namespace WebRTC.iOS
{
    public class FileVideoCapturer : NativeObjectBase, IFileVideoCapturer
    {
        private readonly RTCFileVideoCapturer _capturer;
        private readonly string _file;


        public FileVideoCapturer(IVideoSource videoSource)
        {
            _capturer = new RTCFileVideoCapturer(videoSource.ToNative<IRTCVideoCapturerDelegate>());
            NativeObject = _capturer;
        }

        public FileVideoCapturer(IVideoSource videoSource, string file) : this(videoSource)
        {
            _file = file;
        }


        public bool IsScreencast => false;

        public virtual void StartCapture()
        {
            StartCapturingFromFileNamed(_file);
        }

        public virtual void StartCapture(int videoWidth, int videoHeight, int fps)
        {
            StartCapture();
        }

        public virtual void StartCapturingFromFileNamed(string file)
        {
            _capturer.StartCapturingFromFileNamed(file, (err) => Debug.WriteLine($"FileVideoCapturerNative failed:{err}"));
        }


        public virtual void StopCapture()
        {
            _capturer.StopCapture();
        }
    }
}
