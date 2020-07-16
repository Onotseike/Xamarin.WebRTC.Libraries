// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class RtpTransceiverNative : NativeObjectBase, IRtpTransceiver
    {
        private readonly RtpTransceiver _rtpTransceiver;

        private IRtpSender _rtpSender;

        private IRtpReceiver _rtpReceiver;

        public RtpTransceiverNative(RtpTransceiver rtpTransceiverNative) : base(rtpTransceiverNative)
        {
            _rtpTransceiver = rtpTransceiverNative;
        }

        public RtpMediaType MediaType => _rtpTransceiver.MediaType.ToNet();
        public string Mid => _rtpTransceiver.Mid;
        public bool IsStopped => _rtpTransceiver.IsStopped;

        public RtpTransceiverDirection Direction => _rtpTransceiver.Direction.ToNet();

        public IRtpSender Sender
        {
            get
            {
                if (_rtpTransceiver.Sender == null)
                    return null;
                return _rtpSender ??= new RtpSenderNative(_rtpTransceiver.Sender);
            }
        }

        public IRtpReceiver Receiver
        {
            get
            {
                if (_rtpTransceiver.Receiver == null)
                    return null;
                return _rtpReceiver ??= new RtpReceiverNative(_rtpTransceiver.Receiver);
            }
        }

        public void Stop()
        {
            _rtpTransceiver.Stop();
        }
    }
}