// onotseike@hotmail.comPaula Aliu
using System;
using System.Linq;
using Android.OS;
using Java.IO;
using Java.Lang;
using Org.Webrtc;
using WebRTC.Android.Extensions;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Extensions;
using WebRTC.Interfaces;
using static WebRTC.Classes.DataChannel;
using IceCandidate = WebRTC.Classes.IceCandidate;
using ISdpObserver = WebRTC.Interfaces.ISdpObserver;
using MediaConstraints = WebRTC.Classes.MediaConstraints;
using SessionDescription = WebRTC.Classes.SessionDescription;

namespace WebRTC.Android
{
    internal class PeerConnectionNative : NativeObjectBase, IPeerConnection
    {
        private readonly PeerConnection _peerConnection;


        public PeerConnectionNative(PeerConnection peerConnection, RTCConfiguration configuration,
            IPeerConnectionFactory peerConnectionFactory) : base(peerConnection)
        {
            _peerConnection = peerConnection;
            Configuration = configuration;
            PeerConnectionFactory = peerConnectionFactory;
        }

        public IPeerConnectionFactory PeerConnectionFactory { get; }

        //public IMediaStream[] LocalStreams { get; }
        public SessionDescription LocalDescription => _peerConnection.LocalDescription?.ToNet();
        public SessionDescription RemoteDescription => _peerConnection.RemoteDescription?.ToNet();
        public SignalingState SignalingState => _peerConnection.InvokeSignalingState().ToNet();
        public IceConnectionState IceConnectionState => _peerConnection.InvokeIceConnectionState().ToNet();
        public PeerConnectionState PeerConnectionState => _peerConnection.ConnectionState().ToNet();
        public IceGatheringState IceGatheringState => _peerConnection.InvokeIceGatheringState().ToNet();

        public IRtpSender[] Senders =>
            _peerConnection.Senders.Select(s => new RtpSenderNative(s)).Cast<IRtpSender>().ToArray();

        public IRtpReceiver[] Receivers => _peerConnection.Receivers.Select(s => new RtpReceiverNative(s))
            .Cast<IRtpReceiver>().ToArray();

        public IRtpTransceiver[] Transceivers
        {
            get
            {
                if (Configuration.SdpSemantics != SdpSemantics.UnifiedPlan)
                    throw new InvalidOperationException(
                        "GetTransceivers is only supported with Unified Plan SdpSemantics.");
                return _peerConnection.Transceivers.Select(t => new RtpTransceiverNative(t)).Cast<IRtpTransceiver>()
                    .ToArray();
            }
        }

        public RTCConfiguration Configuration { get; private set; }

        public bool SetConfiguration(RTCConfiguration configuration)
        {
            var result = _peerConnection.SetConfiguration(configuration.ToNative());
            if (result)
                Configuration = configuration;
            return result;
        }

        public void Close()
        {
            _peerConnection.Close();
        }

        public void AddIceCandidate(IceCandidate candidate)
        {
            _peerConnection.AddIceCandidate(candidate.ToNative());
        }

        public void RemoveIceCandidates(IceCandidate[] candidates)
        {
            _peerConnection.RemoveIceCandidates(candidates.Select(c => c.ToNative()).ToArray());
        }

        public void AddStream(IMediaStream stream)
        {
            _peerConnection.AddStream(stream.ToNative<MediaStream>());
        }

        public void RemoveStream(IMediaStream stream)
        {
            _peerConnection.RemoveStream(stream.ToNative<MediaStream>());
        }

        public IRtpSender AddTrack(IMediaStreamTrack track, string[] streamIds)
        {
            var rtpSender = new RtpSenderNative(_peerConnection.AddTrack(track.ToNative(), streamIds));
            return rtpSender;
        }

        public bool RemoveTrack(IRtpSender sender)
        {
            return _peerConnection.RemoveTrack(sender.ToNative<RtpSender>());
        }

        public IRtpTransceiver AddTransceiverWithTrack(IMediaStreamTrack track)
        {
            throw new NotImplementedException();
        }

        public IRtpTransceiver AddTransceiverWithTrack(IMediaStreamTrack track, IRtpTransceiverInit init)
        {
            throw new NotImplementedException();
        }

        public IRtpTransceiver AddTransceiverOfType(RtpMediaType mediaType)
        {
            throw new NotImplementedException();
        }

        public IRtpTransceiver AddTransceiverOfType(RtpMediaType mediaType, IRtpTransceiverInit init)
        {
            throw new NotImplementedException();
        }

        public void CreateOffer(MediaConstraints constraints,
            ISdpObserver observer)
        {
            _peerConnection.CreateOffer(new SdpObserverProxy(observer), constraints.ToNative());
        }

        public void CreateAnswer(MediaConstraints constraints,
            ISdpObserver observer)
        {
            _peerConnection.CreateAnswer(new SdpObserverProxy(observer), constraints.ToNative());
        }

        public void SetLocalDescription(SessionDescription sdp, ISdpObserver observer)
        {
            _peerConnection.SetLocalDescription(new SdpObserverProxy(observer),
                sdp.ToNative());
        }

        public void SetRemoteDescription(SessionDescription sdp, ISdpObserver observer)
        {
            _peerConnection.SetRemoteDescription(new SdpObserverProxy(
                    observer),
                sdp.ToNative());
        }

        public IDataChannel CreateDataChannel(string label, DataChannelConfiguration dataChannelConfiguration)
        {
            var dataChannel = _peerConnection.CreateDataChannel(label, dataChannelConfiguration.ToNative());
            return dataChannel == null ? null : new DataChannelNative(dataChannel);
        }

        public bool SetBitrate(int min, int current, int max)
        {
            return _peerConnection.SetBitrate(new Integer(min), new Integer(current), new Integer(max));
        }

        public bool StartRtcEventLog(string file, int fileSizeLimitBytes)
        {
            try
            {
                ParcelFileDescriptor rtcEventLog = ParcelFileDescriptor.Open(new File(file),
                    ParcelFileMode.Create | ParcelFileMode.Truncate | ParcelFileMode.ReadWrite);

                return _peerConnection.StartRtcEventLog(rtcEventLog.DetachFd(), fileSizeLimitBytes);
            }
            catch (IOException ex)
            {
                return false;
            }
        }

        public void StopRtcEventLog()
        {
            _peerConnection.StopRtcEventLog();
        }
    }
}