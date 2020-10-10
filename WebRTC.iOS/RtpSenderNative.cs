// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal class RtpSenderNative : NativeObjectBase, IRtpSender
    {
        private readonly IRTCRtpSender _rtpSender;

        public RtpSenderNative(IRTCRtpSender rtpSender) : base(rtpSender)
        {
            _rtpSender = rtpSender;
            
        }

        public string SenderId => _rtpSender.SenderId;

        public IMediaStreamTrack Track
        {
            get => _rtpSender.Track.ToNet();
            set => _rtpSender.Track = value.ToNative();
        }
    }
}