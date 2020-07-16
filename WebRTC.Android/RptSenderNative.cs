// onotseike@hotmail.comPaula Aliu
using System;
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class RptSenderNative : NativeObjectBase, IRtpSender
    {
        private readonly RtpSender _rtpSender;

        public RptSenderNative(RtpSender rtpSender) : base(rtpSender)
        {
            _rtpSender = rtpSender;
        }

        public string SenderId => _rtpSender.Id();

        public IMediaStreamTrack Track => _rtpSender.Track()?.ToNet();
    }
}
