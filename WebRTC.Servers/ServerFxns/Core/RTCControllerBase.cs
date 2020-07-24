// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Classes;
using WebRTC.Interfaces;
using WebRTC.Servers.Interfaces;
using WebRTC.Servers.Models;
using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core
{
    public abstract class RTCControllerBase<TConnectionParameters, TSignalParameters> : ISignalingEvents<TSignalParameters>, IPeerConnectionEvents where TConnectionParameters : IConnectionParameters where TSignalParameters : ISignalingParameters
    {
        #region Variables

        private const string TAG = nameof(RTCControllerBase<TConnectionParameters, TSignalParameters>);
        private TSignalParameters signalParameters;

        protected readonly IExecutor Executor;
        protected readonly IRTCEngineEvents RTCEngineEvents;
        protected readonly ILogger Logger;


        #endregion

        #region Properties

        protected IRTCClient<TConnectionParameters> RTCClient { get; private set; }
        protected PeerConnectionClient PeerConnectionClient { get; private set; }

        public bool Connected { get; private set; }

        protected abstract bool IsInitiator { get; }

        protected TSignalParameters SignalParameters { get; private set; }

        #endregion

        #region Constructor(s)

        protected RTCControllerBase(IRTCEngineEvents events, ILogger logger)
        {
            RTCEngineEvents = events;
            Logger = logger;

            Executor = ExecutorServiceFactory.HeadExecutor;
        }

        #endregion

        #region Abstract methods

        protected abstract void OnChannelConnectedInternal(TSignalParameters signalParameters);

        protected abstract IRTCClient<TConnectionParameters> CreateClient();

        protected abstract PeerConnectionParameters CreatePeerConnectionParameters(TSignalParameters signalParameters);

        #endregion

        #region Main Methods

        public void Connect(TConnectionParameters connectionParameters)
        {
            RTCClient = CreateClient();
            RTCClient.Connect(connectionParameters);
        }

        public virtual void Disconnect()
        {
            Connected = false;
            RTCClient?.Disconnect();
            RTCClient = null;
            PeerConnectionClient?.Close();
            PeerConnectionClient = null;
        }

        public void StartVideoCall(IVideoRenderer localRenderer, IVideoRenderer remoteRenderer)
        {
            Executor.Execute(() =>
            {
                PeerConnectionClient.CreatePeerConnection(localRenderer, remoteRenderer);
                OnChannelConnectedInternal(signalParameters);
            });
        }

        public void ChangeCaptureFormat(int width, int height, int framerate) => PeerConnectionClient?.ChangeCaptureFormat(width, height, framerate);

        public void SwitchCamera() => PeerConnectionClient?.SwitchCamera();

        public void SetVideoEnabled(bool enabled) => PeerConnectionClient?.SetVideoEnabled(enabled);

        public void SetAudioEnabled(bool enabled) => PeerConnectionClient?.SetAudioEnabled(enabled);

        protected virtual void OnPeerConnectionCreatedInternal(IPeerConnection peerConnection)
        {

        }

        #endregion

        #region Interface Implements

        public IVideoCapturer CreateVideoCapturer(IPeerConnectionFactory factory, IVideoSource videoSource) => RTCEngineEvents.CreateVideoCapturer(factory, videoSource);

        public void OnChannelClose()
        {
            Executor.Execute(() => RTCEngineEvents.OnDisconnect(DisconnectType.WebSocket));
        }

        public void OnChannelConnected(TSignalParameters signalParam)
        {
            Logger.Debug(TAG, $"Creating PeerConnectionClient");

            SignalParameters = signalParam;
            Executor.Execute(() =>
            {
                signalParameters = signalParam;

                var peerConnectionClientParams = CreatePeerConnectionParameters(signalParam);

                PeerConnectionClient = new PeerConnectionClient(peerConnectionClientParams, this, Logger);
                PeerConnectionClient.CreatePeerConeectionFactory();
                RTCEngineEvents.ReadyToStart();
                Logger.Debug(TAG, $"Created PeerConnectionClient");
            });
        }

        public void OnChannelError(string description)
        {
            Executor.Execute(() => RTCEngineEvents.OnError(description));
        }

        public void OnConnected() => Connected = true;

        public void OnDisconnected()
        {
            Connected = false;
            Executor.Execute(() => { RTCEngineEvents.OnDisconnect(DisconnectType.PeerConnection); });
        }

        public void OnRemoteIceCandidate(IceCandidate candidate)
        {
            Executor.Execute(() =>
            {
                if (PeerConnectionClient == null)
                {
                    Logger.Error(TAG, "Received remote SDP for non-initilized peer connection.");
                    return;
                }

                PeerConnectionClient.AddRemoteIceCandidate(candidate);
            });
        }

        public void OnIceCandidate(IceCandidate candidate)
        {
            Executor.Execute(() => { RTCClient?.SendLocalIceCandidate(candidate); });
        }

        public void OnIceCandidateRemoved(IceCandidate[] candidates)
        {
            Executor.Execute(() => { RTCClient?.SendLocalIceCandidateRemovals(candidates); });
        }

        public void OnLocalDescription(SessionDescription sdp)
        {
            Executor.Execute(() =>
            {
                Logger.Debug(TAG, $"Sending {sdp.Type}");
                if (IsInitiator)
                    RTCClient?.SendSdpOffer(sdp);
                else
                    RTCClient?.SendSdpAnswer(sdp);
            });
        }

        public void OnPeerConnectionCreated(IPeerConnection peerConnection)
        {
            Executor.Execute(() => OnPeerConnectionCreatedInternal(peerConnection));
        }

        public void OnPeerConnectionError(string description)
        {
            Executor.Execute(() => RTCEngineEvents.OnError(description));
        }

        public void OnPeerFactoryCreated(IPeerConnectionFactory factory)
        {
            Executor.Execute(() => { RTCEngineEvents.OnPeerFactoryCreated(factory); });
        }

        public void OnRemoteDescription(SessionDescription sdp)
        {
            Executor.Execute(() =>
            {
                if (PeerConnectionClient == null)
                {
                    Logger.Error(TAG, "Received remote SDP for non-initilized peer connection.");
                    return;
                }

                PeerConnectionClient.SetRemoteDescription(sdp);
            });
        }

        public void OnRemoteIceCandidateRemoved(IceCandidate[] candidates)
        {
            Executor.Execute(() =>
            {
                if (PeerConnectionClient == null)
                {
                    Logger.Error(TAG, "Received remote SDP for non-initilized peer connection.");
                    return;
                }

                PeerConnectionClient.RemoveRemoteIceCandidates(candidates);
            });
        }

        public void OnIceConnected()
        {

        }

        public void OnIceDisconnected()
        {

        }

        public void OnPeerConnectionClosed()
        {

        }

        #endregion
    }
}
