// onotseike@hotmail.comPaula Aliu
using System;
using WebRTC.Classes;
using WebRTC.Enums;
using static WebRTC.Classes.DataChannel;

namespace WebRTC.Interfaces
{
    public delegate void SdpCompletionHandler(SessionDescription sdp, Exception error);


    public interface ISdpObserver
    {
        void OnCreateSuccess(SessionDescription sdp);
        void OnSetSuccess();
        void OnCreateFailure(string error);
        void OnSetFailure(string error);
    }

    public interface IPeerConnection : INativeObject
    {

        IPeerConnectionFactory PeerConnectionFactory { get; }

        //IMediaStream[] LocalStreams { get; }
        SessionDescription LocalDescription { get; }
        SessionDescription RemoteDescription { get; }

        SignalingState SignalingState { get; }

        IceConnectionState IceConnectionState { get; }

        PeerConnectionState PeerConnectionState { get; }

        IceGatheringState IceGatheringState { get; }

        IRtpSender[] Senders { get; }

        IRtpReceiver[] Receivers { get; }

        IRtpTransceiver[] Transceivers { get; }

        RTCConfiguration Configuration { get; }

        bool SetConfiguration(RTCConfiguration configuration);

        void Close();

        void AddIceCandidate(IceCandidate candidate);
        void RemoveIceCandidates(IceCandidate[] candidates);
        void AddStream(IMediaStream stream);
        void RemoveStream(IMediaStream stream);

        IRtpSender AddTrack(IMediaStreamTrack track, string[] streamIds);

        bool RemoveTrack(IRtpSender sender);

        IRtpTransceiver AddTransceiverWithTrack(IMediaStreamTrack track);
        IRtpTransceiver AddTransceiverWithTrack(IMediaStreamTrack track, IRtpTransceiverInit init);

        IRtpTransceiver AddTransceiverOfType(RtpMediaType mediaType);
        IRtpTransceiver AddTransceiverOfType(RtpMediaType mediaType, IRtpTransceiverInit init);

        void CreateOffer(MediaConstraints constraints, ISdpObserver observer);

        void CreateAnswer(MediaConstraints constraints, ISdpObserver observer);

        void SetLocalDescription(SessionDescription sdp, ISdpObserver observer);

        void SetRemoteDescription(SessionDescription sdp, ISdpObserver observer);

        IDataChannel CreateDataChannel(string label, DataChannelConfiguration dataChannelConfiguration);

        bool SetBitrate(int min, int current, int max);
        bool StartRtcEventLog(string file, int fileSizeLimitBytes);
        void StopRtcEventLog();
    }

    public interface IRtpTransceiverInit
    {
    }
}
