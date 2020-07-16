// onotseike@hotmail.comPaula Aliu
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class RtpReceiverNative : NativeObjectBase, IRtpReceiver
    {
        private readonly RtpReceiver _receiver;

        public RtpReceiverNative(RtpReceiver receiver) : base(receiver)
        {
            _receiver = receiver;
        }

        public string Id => _receiver.Id();
        public IMediaStreamTrack Track => _receiver.Track().ToNet();
    }
}