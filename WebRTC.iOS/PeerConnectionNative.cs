// onotseike@hotmail.comPaula Aliu
using System;
using System.Linq;
using Foundation;
using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Extensions;
using WebRTC.Interfaces;
using WebRTC.iOS.Binding;
using WebRTC.iOS.Extensions;
using static WebRTC.Classes.DataChannel;

namespace WebRTC.iOS
{
    internal class PeerConnectionNative : NativeObjectBase, IPeerConnection
    {
        private readonly RTCPeerConnection _peerConnection;

        public PeerConnectionNative(RTCPeerConnection peerConnection, Classes.RTCConfiguration configuration, IPeerConnectionFactory factory)
        {
            _peerConnection = peerConnection;
            Configuration = configuration;
            PeerConnectionFactory = factory;
        }

        public IPeerConnectionFactory PeerConnectionFactory { get; }
        public SessionDescription LocalDescription => _peerConnection.LocalDescription?.ToNet();
        public SessionDescription RemoteDescription => _peerConnection.RemoteDescription?.ToNet();
        public SignalingState SignalingState => _peerConnection.SignalingState.ToNet();
        public IceConnectionState IceConnectionState => _peerConnection.IceConnectionState.ToNet();
        public PeerConnectionState PeerConnectionState => _peerConnection.ConnectionState.ToNet();
        public IceGatheringState IceGatheringState => _peerConnection.IceGatheringState.ToNet();

        public IRtpSender[] Senders =>
            _peerConnection.Senders.Select(s => new RtpSenderNative(s)).Cast<IRtpSender>().ToArray();

        public IRtpReceiver[] Receivers =>
            _peerConnection.Receivers.Select(r => new RtpReceiverNative(r)).Cast<IRtpReceiver>().ToArray();


        public IRtpTransceiver[] Transceivers
        {
            get
            {
                if (Configuration.SdpSemantics != SdpSemantics.UnifiedPlan)
                    throw new InvalidOperationException("GetTransceivers is only supported with Unified Plan SdpSemantics.");
                return _peerConnection.Transceivers.Select(t => new RtpTransceiverNative(t)).Cast<IRtpTransceiver>().ToArray();
            }
        }

        public Classes.RTCConfiguration Configuration { get; private set; }

        public bool SetConfiguration(Classes.RTCConfiguration configuration)
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
            _peerConnection.RemoveIceCandidates(candidates.ToNative().ToArray());
        }

        public void AddStream(IMediaStream stream)
        {
            _peerConnection.AddStream(stream.ToNative<RTCMediaStream>());
        }

        public void RemoveStream(IMediaStream stream)
        {
            _peerConnection.RemoveStream(stream.ToNative<RTCMediaStream>());
        }

        public IRtpSender AddTrack(IMediaStreamTrack track, string[] streamIds)
        {
            var rtpSender = _peerConnection.AddTrack(track.ToNative<RTCMediaStreamTrack>(), streamIds);
            if (rtpSender == null)
                return null;
            return new RtpSenderNative(rtpSender);
        }

        public bool RemoveTrack(IRtpSender sender)
        {
            return _peerConnection.RemoveTrack(sender.ToNative<IRTCRtpSender>());
        }

        public IRtpTransceiver AddTransceiverWithTrack(IMediaStreamTrack track)
        {
            var rtpTransceiver = _peerConnection.AddTransceiverWithTrack(track.ToNative<RTCMediaStreamTrack>());
            if (rtpTransceiver == null)
                return null;
            return new RtpTransceiverNative(rtpTransceiver);
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

        public void CreateOffer(MediaConstraints constraints, ISdpObserver observer)
        {
            var sdpCallbacksHelper = new SdpCallbackHelper(observer);

            _peerConnection.OfferForConstraints(constraints.ToNative(), sdpCallbacksHelper.CreateSdp);
        }

        public void CreateAnswer(MediaConstraints constraints, ISdpObserver observer)
        {
            var sdpCallbacksHelper = new SdpCallbackHelper(observer);

            _peerConnection.AnswerForConstraints(constraints.ToNative(), sdpCallbacksHelper.CreateSdp);
        }

        public void SetLocalDescription(SessionDescription sdp, ISdpObserver observer)
        {
            var sdpCallbacksHelper = new SdpCallbackHelper(observer);

            _peerConnection.SetLocalDescription(sdp.ToNative(), sdpCallbacksHelper.SetSdp);

        }

        public void SetRemoteDescription(SessionDescription sdp, ISdpObserver observer)
        {
            var sdpCallbacksHelper = new SdpCallbackHelper(observer);

            _peerConnection.SetRemoteDescription(sdp.ToNative(), sdpCallbacksHelper.SetSdp);
        }

        public IDataChannel CreateDataChannel(string label, DataChannelConfiguration dataChannelConfiguration)
        {
            var dataChannel = _peerConnection.DataChannelForLabel(label, dataChannelConfiguration.ToNative());
            return dataChannel == null ? null : new DataChannelNative(dataChannel);
        }

        public bool SetBitrate(int min, int current, int max)
        {
            return _peerConnection.SetBweMinBitrateBps(new NSNumber(min), new NSNumber(current), new NSNumber(max));
        }

        public bool StartRtcEventLogWithFilePath(string filePath, long maxSizeInBytes)
        {
            return _peerConnection.StartRtcEventLogWithFilePath(filePath, maxSizeInBytes);
        }

        public void StopRtcEventLog()
        {
            _peerConnection.StopRtcEventLog();
        }

        public bool StartRtcEventLog(string file, int fileSizeLimitBytes)
        {
            throw new NotImplementedException();
        }



        private class SdpCallbackHelper
        {
            private readonly ISdpObserver _observer;

            public SdpCallbackHelper(ISdpObserver observer)
            {
                _observer = observer;
            }

            public void SetSdp(NSError error)
            {
                if (error != null)
                    _observer?.OnSetFailure(error.LocalizedDescription);
                else
                    _observer?.OnSetSuccess();
            }

            public void CreateSdp(RTCSessionDescription sdp, NSError error)
            {

                if (error != null)
                {
                    _observer?.OnCreateFailure(error.LocalizedDescription);
                }
                else
                {
                    _observer?.OnCreateSuccess(sdp.ToNet());
                }
            }
        }
    }
}