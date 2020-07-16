// onotseike@hotmail.comPaula Aliu
using WebRTC.Classes;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;

namespace WebRTC.iOS
{
    internal class RtpReceiverNative : NativeObjectBase, IRtpReceiver
    {
        private readonly IRTCRtpReceiver _rtpReceiver;
        public RtpReceiverNative(IRTCRtpReceiver rtpReceiver) : base(rtpReceiver)
        {
            _rtpReceiver = rtpReceiver;
        }

        public string Id => _rtpReceiver.ReceiverId;
        public IMediaStreamTrack Track => _rtpReceiver.Track.ToNet();
    }
}