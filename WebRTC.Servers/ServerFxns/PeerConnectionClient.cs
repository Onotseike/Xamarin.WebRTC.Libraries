// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Interfaces;
using WebRTC.Servers.Interfaces;
using WebRTC.Servers.Models;

namespace WebRTC.Servers.ServerFxns
{
    public class PeerConnectionClient
    {
        public const string VideoTrackId = "ARDAMSv0";
        public const string AudioTrackId = "ARDAMSa0";
        public const string VideoTrackType = "video";

        private const string TAG = nameof(PeerConnectionClient);

        private const string AudioEchoCancellationConstraint = "googEchoCancellation";
        private const string AudioAutoGainControlConstraint = "googAutoGainControl";
        private const string AudioHighPassFilterConstraint = "googHighpassFilter";
        private const string AudioNoiseSuppressionConstraint = "googNoiseSuppression";

        private const int HDVideoWidth = 1280;
        private const int HDVideoHeight = 720;

        private readonly SdpObserver sdpObserver;
        private readonly IPeerConnectionListener peerConnectionListener;
        private readonly PeerConnectionParameters peerConnectionParameters;
        private readonly IPeerConnectionEvents peerConnectionEvents;

        private readonly IExecutorService executorService;
        private readonly ILogger logger;

        private IPeerConnectionFactory peerConnectionFactory;
        private IPeerConnection peerConnection;
        private IList<IceCandidate> queuedRemoteIceCandidates;

        private bool renderVideo = true;
        private bool enableAudio = true;

        private IVideoRenderer localVideoRenderer;
        private IVideoRenderer remoteVideoRenderer;

        private IVideoCapturer videoCapturer;
        private bool isVideoCapturerStopped;

        private IAudioSource audioSource;
        private IAudioTrack localAudioTrack;

        private IVideoSource videoSource;
        private IVideoTrack localVideoTrack;
        private IVideoTrack remoteVideoTrack;

        private IRtpSender localVideoSender;

        private int videoHeight;
        private int videoWidth;
        private int fps;

        private MediaConstraints audioConstraints;
        private MediaConstraints sdpMediaContraints;

        private SessionDescription localSdp;

        private RTCEventLog rtcEventLog;
        private bool isInitiator;

        private bool isError;

        private bool IsVideoCallEnabled => peerConnectionParameters.IsVideoCallEnabled;


        public PeerConnectionClient(PeerConnectionParameters parameters, IPeerConnectionEvents events, ILogger _logger = null)
        {
            peerConnectionParameters = parameters;
            peerConnectionEvents = events;

            executorService = ExecutorServiceFactory.CreateExecutorService(TAG);

            logger = _logger ?? new ConsoleLogger();

            sdpObserver = new SdpObserver(this);
            peerConnectionListener = new PeerConnectionListener(this);
        }

        #region Main Functions(Public)

        public void SetVideoEnabled(bool enabled)
        {
            executorService.Execute(() =>
            {
                renderVideo = enabled;
                if (localVideoTrack != null)
                {
                    localVideoTrack.IsEnabled = renderVideo;
                }
                if (remoteVideoTrack != null)
                {
                    remoteVideoTrack.IsEnabled = renderVideo;
                }
            });
        }

        public void SetAudioEnabled(bool enabled)
        {
            executorService.Execute(() =>
            {
                enableAudio = enabled;
                if (localAudioTrack != null)
                {
                    localAudioTrack.IsEnabled = enableAudio;
                }
            });
        }

        public void Close() => executorService.Execute(CloseInternal);

        public void CreatePeerConeectionFactory()
        {
            if (peerConnectionFactory != null)
            {
                throw new InvalidOperationException("Peer Connection Factory has already been Created");
            }

            executorService.Execute(() =>
            {
                peerConnectionFactory = new PeerConnectionFactory();
                peerConnectionEvents.OnPeerFactoryCreated(peerConnectionFactory);
            });
        }

        public void CreatePeerConnection(IVideoRenderer localRenderer, IVideoRenderer remoteRenderer)
        {
            localVideoRenderer = localRenderer;
            remoteVideoRenderer = remoteRenderer;

            executorService.Execute(() =>
            {
                try
                {
                    CreateMediaConstraintsInternal();
                    CreatePeerConnectionInternal();
                    MaybeCreatedAndStartRTCEventLog();
                }
                catch (Exception ex)
                {
                    ReportError($"FAILED TO CREATE PEER CONNECTION PIPLINE: {ex.Message}");
                    throw;
                }
            });
        }

        public void CreateOffer()
        {
            executorService.Execute(() =>
            {
                if (peerConnection == null || isError) return;

                logger.Debug(TAG, $"Peer Connection Create OFFER");
                isInitiator = false;
                peerConnection.CreateOffer(sdpMediaContraints, sdpObserver);
            });
        }

        public void CreateAnswer()
        {
            executorService.Execute(() =>
            {
                if (peerConnection == null || isError) return;

                logger.Debug(TAG, $"Peer Connection Create ANSWER");
                isInitiator = false;
                peerConnection.CreateAnswer(sdpMediaContraints, sdpObserver);
            });
        }

        public void AddRemoteIceCandidate(IceCandidate iceCandidate)
        {
            executorService.Execute(() =>
            {
                if (peerConnection == null || isError) return;

                if (queuedRemoteIceCandidates != null)
                {
                    queuedRemoteIceCandidates.Add(iceCandidate);
                }
                else
                {
                    peerConnection.AddIceCandidate(iceCandidate);
                }
            });
        }

        public void RemoveRemoteIceCandidates(IceCandidate[] iceCandidates)
        {
            executorService.Execute(() =>
            {
                if (peerConnection == null || isError) return;

                DrainCandidates();
                peerConnection.RemoveIceCandidates(iceCandidates);
            });
        }

        public void SetRemoteDescription(SessionDescription sdp)
        {
            executorService.Execute(() =>
            {
                if (peerConnection == null || isError) return;

                logger.Debug(TAG, $"Set Remote SDP.");
                peerConnection.SetRemoteDescription(sdp, sdpObserver);
            });
        }

        public void StopVideoSource()
        {
            executorService.Execute(() =>
            {
                if (videoCapturer != null && !isVideoCapturerStopped)
                {
                    logger.Debug(TAG, $"Stopping Video Source");
                    videoCapturer.StopCapture();
                }

                isVideoCapturerStopped = true;
            });
        }

        public void StartVideoSource()
        {
            executorService.Execute(() =>
            {
                if (videoCapturer == null || isVideoCapturerStopped) return;

                logger.Debug(TAG, $"Restart Video Source");
                videoCapturer.StartCapture(videoWidth, videoHeight, fps);
                isVideoCapturerStopped = false;
            });
        }

        public void SwitchCamera()
        {
            executorService.Execute(() =>
            {
                if (videoCapturer is ICameraVideoCapturer cameraVideoCapturer)
                {
                    if (!IsVideoCallEnabled || isError)
                    {
                        logger.Error(TAG, $"Failed to switch Camera. Video: {IsVideoCallEnabled}. Error: {isError}.");
                        return;
                    }

                    logger.Debug(TAG, $"Switch Camera.");
                    cameraVideoCapturer.SwitchCamera();
                }
                else
                {
                    logger.Debug(TAG, $"Will not Switch Camera, Video Capturer is not a Camera");
                }
            });
        }

        public void ChangeCaptureFormat(int width, int height, int framerate)
        {
            executorService.Execute(() =>
            {
                if (!IsVideoCallEnabled || isError || videoCapturer == null)
                {
                    logger.Error(TAG, $"Failed to change capture format. Video: {IsVideoCallEnabled}. Error : {isError} or VideoCapturer is NULL");
                    return;
                }

                logger.Error(TAG, $"ChangeCaptureFormat: {width} | {height} @ {framerate}");
                videoSource.AdaptOutputFormat(width, height, framerate);
            });
        }

        #endregion

        #region Helper Functions (Private)


        private void CloseInternal()
        {
            if (peerConnectionFactory != null && peerConnectionParameters.AECDump)
            {
                peerConnectionFactory.StopAecDump();
            }

            logger.Debug(TAG, $"Closing Peer Connection.");
            if (rtcEventLog != null)
            {
                rtcEventLog.Stop();
                rtcEventLog = null;
            }

            logger.Debug(TAG, $"Closing Audio Source.");
            if (audioSource != null)
            {
                audioSource.Dispose();
                audioSource = null;
            }

            logger.Debug(TAG, $"Stopping Capturer.");
            if (videoCapturer != null)
            {
                videoCapturer.StopCapture();
                videoCapturer.Dispose();
                isVideoCapturerStopped = true;
                videoCapturer = null;
            }

            logger.Debug(TAG, $"Closing Video Source.");
            if (videoSource != null)
            {
                videoSource.Dispose();
                videoSource = null;
            }

            localVideoRenderer = null;
            remoteVideoRenderer = null;
            logger.Debug(TAG, $"Closing Peer Connection Factory. ");
            if (peerConnectionFactory != null)
            {
                peerConnectionFactory.Dispose();
                peerConnectionFactory = null;
            }

            logger.Debug(TAG, $"Closing Peer Coneection Done.");
            peerConnectionEvents.OnPeerConnectionClosed();
            PeerConnectionFactory.StopInternalTracingCapture();
            PeerConnectionFactory.ShutdownInternalTracer();

            executorService.Release();
        }

        private void CreateMediaConstraintsInternal()
        {
            if (IsVideoCallEnabled)
            {
                videoWidth = peerConnectionParameters.VideoWidth;
                videoHeight = peerConnectionParameters.VideoHeight;
                fps = peerConnectionParameters.VideoFPS;

                if (videoHeight == 0 || videoWidth == 0)
                {
                    videoWidth = HDVideoWidth;
                    videoHeight = HDVideoHeight;
                }

                if (fps == 0)
                {
                    fps = 30;
                }

                logger.Debug(TAG, $"Capturing Format: {videoWidth} | {videoHeight} @ {fps}");
            }

            audioConstraints = new MediaConstraints();
            if (peerConnectionParameters.NoAudioProcessing)
            {
                logger.Debug(TAG, "Disabling Audio Processing");
                audioConstraints.Mandatory.Add(AudioEchoCancellationConstraint, "false");
                audioConstraints.Mandatory.Add(AudioAutoGainControlConstraint, "false");
                audioConstraints.Mandatory.Add(AudioHighPassFilterConstraint, "false");
                audioConstraints.Mandatory.Add(AudioNoiseSuppressionConstraint, "false");
            }

            sdpMediaContraints = new MediaConstraints();
            sdpMediaContraints.Mandatory.Add("OfferToReceiveAudio", "true");
            sdpMediaContraints.Mandatory.Add("OfferToReceiveVideo", peerConnectionParameters.IsVideoCallEnabled ? "true" : "false");
        }

        private void CreatePeerConnectionInternal()
        {
            if (peerConnectionFactory == null || isError)
            {
                logger.Error(TAG, $"Peer Connection Factory is not created.");
                return;
            }

            logger.Debug(TAG, $"Create Peer Connection.");
            queuedRemoteIceCandidates = new List<IceCandidate>();

            var rtcConfig = new RTCConfiguration(peerConnectionParameters.IceServers);
            rtcConfig.TcpCandidatePolicy = TcpCandidatePolicy.Disabled;
            rtcConfig.BundlePolicy = BundlePolicy.MaxBundle;
            rtcConfig.RtcpMuxPolicy = RtcpMuxPolicy.Require;
            rtcConfig.ContinualGatheringPolicy = ContinualGatheringPolicy.Continually;

            // ECDSA Encryption
            rtcConfig.KeyType = EncryptionKeyType.Ecdsa;
            rtcConfig.EnableDtlsSrtp = peerConnectionParameters.LoopBack;
            rtcConfig.SdpSemantics = SdpSemantics.UnifiedPlan;

            peerConnection = peerConnectionFactory.CreatePeerConnection(rtcConfig, peerConnectionListener);

            var mediaStreamLabels = new[] { "ARDAMS" };

            if (IsVideoCallEnabled)
            {
                peerConnection.AddTrack(CreateVideoTrack(), mediaStreamLabels);

                remoteVideoTrack = GetRemoteVideoTrack();
                remoteVideoTrack.IsEnabled = renderVideo;
                remoteVideoTrack.AddRenderer(remoteVideoRenderer);
            }

            peerConnection.AddTrack(CreateAudioTrack(), mediaStreamLabels);

            if (IsVideoCallEnabled)
            {
                FindVideoSender();
            }

            if (peerConnectionParameters.AECDump)
            {
                var result = peerConnectionFactory.StartAecDump(peerConnectionParameters.AEcDumpFile, -1);
                if (!result)
                {
                    logger.Error(TAG, $"Can not open AEC Dump File");
                }
            }

            logger.Debug(TAG, $"Peer Connection was Created.");
        }

        private void MaybeCreatedAndStartRTCEventLog()
        {
            if (peerConnection == null) return;

            if (!peerConnectionParameters.EnableRTCEventLog)
            {
                logger.Debug(TAG, $"RTCEventLogging is Disabled");
                return;
            }

            var file = Path.Combine(peerConnectionParameters.RTCEventLogDirectory, CreateRTCEventLogOutputFile());
            rtcEventLog = new RTCEventLog(peerConnection, file, logger);
            rtcEventLog.Start();
        }

        private string CreateRTCEventLogOutputFile() => $"EVENT_LOG_{DateTime.Now:yyyyMMdd_hhmm_ss}";

        private IAudioTrack CreateAudioTrack()
        {
            audioSource = peerConnectionFactory.CreateAudioSource(audioConstraints);
            localAudioTrack = peerConnectionFactory.CreateAudioTrack(AudioTrackId, audioSource);
            localAudioTrack.IsEnabled = enableAudio;
            return localAudioTrack;
        }

        private IVideoTrack CreateVideoTrack()
        {
            videoSource = peerConnectionFactory.CreateVideoSource(peerConnectionParameters.IsScreenCast);
            videoCapturer = peerConnectionEvents.CreateVideoCapturer(peerConnectionFactory, videoSource);

            videoCapturer.StartCapture(videoWidth: videoWidth, videoHeight: videoHeight, fps: fps);
            localVideoTrack = peerConnectionFactory.CreateVideoTrack(VideoTrackId, videoSource);

            localVideoTrack.IsEnabled = renderVideo;
            localVideoTrack.AddRenderer(localVideoRenderer);

            return localVideoTrack;
        }

        private IVideoTrack GetRemoteVideoTrack()
        {
            foreach (var transceiver in peerConnection.Transceivers)
            {
                if (transceiver.Receiver.Track is IVideoTrack videoTrack) return videoTrack;
            }
            return null;
        }

        private void FindVideoSender()
        {
            foreach (var sender in peerConnection.Senders)
            {
                if (sender.Track == null) continue;

                if (VideoTrackType.Equals(sender.Track.Kind))
                {
                    logger.Debug(TAG, $"Found Video Sender.");
                    localVideoSender = sender;
                }
            }
        }

        private void ReportError(string description)
        {
            logger.Error(TAG, $"PeerConnectionError: {description}");
            executorService.Execute(() =>
            {
                if (isError) return;

                peerConnectionEvents.OnPeerConnectionError(description);
                isError = true;
            });
        }

        private void DrainCandidates()
        {
            if (queuedRemoteIceCandidates == null) return;

            logger.Debug(TAG, $"Add {queuedRemoteIceCandidates.Count} remote candidates");
            foreach (var candidate in queuedRemoteIceCandidates)
            {
                peerConnection.AddIceCandidate(candidate);
            }
            queuedRemoteIceCandidates = null;
        }

        #endregion

        #region SdpObserver Class

        private class SdpObserver : ISdpObserver
        {
            private readonly PeerConnectionClient peerConnectionClient;
            private readonly IPeerConnectionEvents peerConnectionEvents;
            private readonly IExecutor executor;
            private readonly ILogger logger;

            private bool IsError => peerConnectionClient.isError;
            private IPeerConnection PeerConnection => peerConnectionClient.peerConnection;

            private SessionDescription LocalSdp
            {
                get => peerConnectionClient.localSdp;
                set => peerConnectionClient.localSdp = value;
            }

            private bool IsInitiator => peerConnectionClient.isInitiator;

            public SdpObserver(PeerConnectionClient _peerConnectionClient)
            {
                peerConnectionClient = _peerConnectionClient;
                executor = _peerConnectionClient.executorService;
                logger = _peerConnectionClient.logger;
                peerConnectionEvents = _peerConnectionClient.peerConnectionEvents;
            }


            public void OnCreateFailure(string error)
            {
                ReportError($"CreateSDP error: {error}");
            }

            public void OnCreateSuccess(SessionDescription sdp)
            {
                if (LocalSdp != null)
                {
                    ReportError($"Multiple SDPs created.");
                    return;
                }

                LocalSdp = sdp;
                executor.Execute(() =>
                {
                    if (PeerConnection == null || IsError) return;

                    logger.Debug(TAG, $"Set Local SDP from {sdp.Type}");
                    PeerConnection.SetLocalDescription(sdp, this);
                });
            }

            public void OnSetFailure(string error)
            {
                ReportError($"SetSDP error: {error}");
            }

            public void OnSetSuccess()
            {
                executor.Execute(() =>
                {
                    if (PeerConnection == null || IsError) return;

                    if (IsInitiator)
                    {
                        // For offering a Peer Connection, we do the following in order:
                        // Create Offer
                        // Set Local SDP
                        // After recieving answer, Set Remote SDP

                        if (PeerConnection.RemoteDescription == null)
                        {
                            logger.Debug(TAG, "Local SDP was set successfully");
                            peerConnectionEvents.OnLocalDescription(LocalSdp);
                        }
                        else
                        {
                            peerConnectionClient.DrainCandidates();
                        }

                    }
                    else
                    {
                        if (PeerConnection.LocalDescription != null)
                        {
                            logger.Debug(TAG, "Local SDP set successfully");
                            peerConnectionEvents.OnLocalDescription(LocalSdp);
                            peerConnectionClient.DrainCandidates();
                        }
                    }
                });
            }

            private void ReportError(string description)
            {
                peerConnectionClient.ReportError(description);
            }

        }

        #endregion

        #region PeerConnectionListener Class

        private class PeerConnectionListener : IPeerConnectionListener
        {
            private readonly PeerConnectionClient peerConnectionClient;
            private readonly ILogger logger;
            private readonly IExecutor executor;
            private readonly IPeerConnectionEvents peerConnectionEvents;

            public PeerConnectionListener(PeerConnectionClient _peerConnectionClient)
            {
                peerConnectionClient = _peerConnectionClient;
                executor = _peerConnectionClient.executorService;
                peerConnectionEvents = _peerConnectionClient.peerConnectionEvents;
                logger = _peerConnectionClient.logger;
            }

            public void OnAddStream(IMediaStream mediaStream)
            {

            }

            public void OnRemoveStream(IMediaStream mediaStream)
            {

            }

            public void OnDataChannel(DataChannel.IDataChannel dataChannel)
            {

            }

            public void OnRenegotiationNeeded()
            {

            }

            public void OnAddTrack(IRtpReceiver rtpReceiver, IMediaStream[] mediaStreams)
            {

            }

            public void OnTrack(IRtpTransceiver transceiver)
            {

            }


            public void OnConnectionChange(PeerConnectionState newState)
            {
                executor.Execute(() =>
                {
                    logger.Debug(TAG, $"PeerConnectionState: {newState}");
                    switch (newState)
                    {
                        case PeerConnectionState.Connected:
                            peerConnectionEvents.OnConnected();
                            break;
                        case PeerConnectionState.Disconnected:
                            peerConnectionEvents.OnDisconnected();
                            break;
                        case PeerConnectionState.Failed:
                            peerConnectionClient.ReportError($"DTLS connection failed");
                            break;
                    }
                });
            }


            public void OnIceCandidate(IceCandidate iceCandidate)
            {
                executor.Execute(() => peerConnectionEvents.OnIceCandidate(iceCandidate));
            }

            public void OnIceCandidatesRemoved(IceCandidate[] iceCandidates)
            {
                executor.Execute(() => peerConnectionEvents.OnIceCandidateRemoved(iceCandidates));
            }

            public void OnIceConnectionChange(IceConnectionState iceConnectionState)
            {
                executor.Execute(() =>
                {
                    logger.Debug(TAG, $"IceConnectionState: {iceConnectionState}");
                    switch (iceConnectionState)
                    {
                        case IceConnectionState.Connected:
                            peerConnectionEvents.OnIceConnected();
                            break;
                        case IceConnectionState.Disconnected:
                            peerConnectionEvents.OnIceDisconnected();
                            break;
                        case IceConnectionState.Failed:
                            peerConnectionClient.ReportError($"Ice connection failed");
                            break;
                    }
                });
            }

            public void OnIceGatheringChange(IceGatheringState iceGatheringState)
            {
                logger.Debug(TAG, $"IceGatheringState: {iceGatheringState}");
            }

            public void OnSignalingChange(SignalingState signalingState)
            {
                logger.Debug(TAG, $"SignalingState: {signalingState}");
            }


        }

        #endregion
    }
}
