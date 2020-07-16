// onotseike@hotmail.comPaula Aliu
using System.Linq;
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Interfaces;

namespace WebRTC.Android
{
    internal class PeerConnectionListenerProxy : Java.Lang.Object, PeerConnection.IObserver
    {
        private readonly IPeerConnectionListener _listener;

        public PeerConnectionListenerProxy(IPeerConnectionListener listener)
        {
            _listener = listener;
        }

        public void OnAddStream(MediaStream p0)
        {
            _listener.OnAddStream(new MediaStreamNative(p0));
        }

        public void OnAddTrack(RtpReceiver p0, MediaStream[] p1)
        {
            _listener.OnAddTrack(new RtpReceiverNative(p0), ConvertToNative(p1));
        }

        public void OnDataChannel(DataChannel p0)
        {
            _listener.OnDataChannel(new DataChannelNative(p0));
        }

        public void OnIceCandidate(IceCandidate p0)
        {
            _listener.OnIceCandidate(p0.ToNet());
        }

        public void OnIceCandidatesRemoved(IceCandidate[] p0)
        {
            _listener.OnIceCandidatesRemoved(p0.Select(p => p.ToNet()).ToArray());
        }

        public void OnIceConnectionChange(PeerConnection.IceConnectionState p0)
        {
            _listener.OnIceConnectionChange(p0.ToNet());
        }

        public void OnIceConnectionReceivingChange(bool p0)
        {
            // _handler.Post(() =>_listener.ice
        }

        public void OnIceGatheringChange(PeerConnection.IceGatheringState p0)
        {
            _listener.OnIceGatheringChange(p0.ToNet());
        }

        public void OnRemoveStream(MediaStream p0)
        {
            _listener.OnRemoveStream(new MediaStreamNative(p0));
        }

        public void OnRenegotiationNeeded()
        {
            _listener.OnRenegotiationNeeded();
        }

        public void OnSignalingChange(PeerConnection.SignalingState p0)
        {
            _listener.OnSignalingChange(p0.ToNet());
        }

        private static IMediaStream[] ConvertToNative(MediaStream[] source)
        {
            var arr = new IMediaStream[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                arr[i] = new MediaStreamNative(source[i]);
            }

            return arr;
        }
    }
}