// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class RtpSenderNative : NativeObjectBase, IRtpSender
    {
        private readonly RtpSender _rtpSender;

        public RtpSenderNative(RtpSender nativeRtpSender) : base(nativeRtpSender)
        {
            _rtpSender = nativeRtpSender;
        }
        public string SenderId => _rtpSender.Id();

        public IMediaStreamTrack Track
        {
            get => _rtpSender.Track().ToNet();
            set => _rtpSender.SetTrack(value.ToNative(), true);
        }
    }
}