// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.RTC.Abstraction;

namespace WebRTC.RTC
{
    public class RTCClient : IRTCClient<RoomConnectionParameters>, IWebSocketChannelEvents
    {
        private enum MessageType
        {
            Message,
            Leave
        }

        private const string TAG = nameof(RTCClient);
        private const string ROOM_JOIN = "join";
        private const string ROOM_MESSAGE = "message";
        private const string ROOM_LEAVE = "leave";

        private readonly ISignalingEvents<SignalingParameters> _signalingEvents;
        private readonly IExecutorService _executor;
        private readonly ILogger _logger;

        private WebSocketChannelClient _wsClient;

        private RoomConnectionParameters _connectionParameters;

        private bool _initiator;
        private string _messageUrl;
        private string _leaveUrl;
        

        public RTCClient(ISignalingEvents<SignalingParameters> signalingEvents, ILogger logger = null)
        {
            _signalingEvents = signalingEvents;
            _executor = ExecutorServiceFactory.CreateExecutorService(nameof(RTCClient));
            _logger = logger ?? new ConsoleLogger();
            State = ConnectionState.New;

        }

        public ConnectionState State { get; private set; }


        public async void Connect(RoomConnectionParameters connectionParameters)
        {
            _connectionParameters = connectionParameters;
            _executor.Execute(ConnectToRoomInternal);
        }

        public void Disconnect()
        {
            _executor.Execute(DisconnectFromRoomInternal);
        }

        public void SendOfferSdp(SessionDescription sdp)
        {
            
            _executor.Execute(() =>
            {
                if (State != ConnectionState.Connected)
                {
                    ReportError("Sending offer SDP in non connected state.");
                    return;
                }

                var json = SignalingMessage.CreateJson(sdp);

                SendPostMessage(MessageType.Message, _messageUrl, json);

                if (_connectionParameters.IsLoopback)
                {
                    // In loopback mode rename this offer to answer and route it back.
                    var sdpAnswer = new SessionDescription(SdpType.Answer, sdp.Sdp);
                    _signalingEvents.OnRemoteDescription(sdpAnswer);
                }
            });
        }

        public void SendAnswerSdp(SessionDescription sdp)
        {
            _executor.Execute(() =>
            {
                if (_connectionParameters.IsLoopback)
                {
                    _logger.Error(TAG, "Sending answer in loopback mode.");
                    return;
                }

                var json = SignalingMessage.CreateJson(sdp);


                _wsClient.Send(json);
            });
        }

        public void SendLocalIceCandidate(IceCandidate candidate)
        {
            _executor.Execute(() =>
            {
                var json = SignalingMessage.CreateJson(candidate);
                if (_initiator)
                {
                    if (State != ConnectionState.Connected)
                    {
                        ReportError("Sending ICE candidate in non connected state.");
                        return;
                    }

                    SendPostMessage(MessageType.Message, _messageUrl, json);
                }
                else
                {
                    _wsClient.Send(json);
                }
            });
        }

        public void SendLocalIceCandidateRemovals(IceCandidate[] candidates)
        {
            _executor.Execute(() =>
            {
                var json = SignalingMessage.CreateJson(candidates);

                if (_initiator)
                {
                    if (State != ConnectionState.Connected)
                    {
                        ReportError("Sending ICE candidate removals in non connected state.");
                        return;
                    }

                    SendPostMessage(MessageType.Message, _messageUrl, json);
                    if (_connectionParameters.IsLoopback)
                    {
                        _signalingEvents.OnRemoteIceCandidatesRemoved(candidates);
                    }
                }
                else
                {
                    _wsClient.Send(json);
                }
            });
        }

        public void OnWebSocketClose()
        {
            _signalingEvents.OnChannelClose();
        }

        public void OnWebSocketMessage(string message)
        {
            if (_wsClient.State != WebSocketConnectionState.Registered)
            {
                _logger.Error(TAG, "Got WebSocket message in non registered state.");
                return;
            }

            var msg = SignalingMessage.MessageFromJSONString(message);

            switch (msg.Type)
            {
                case SignalingMessageType.Candidate:
                    var candidate = (ICECandidateMessage)msg;
                    _signalingEvents.OnRemoteIceCandidate(candidate.Candidate);
                    break;
                case SignalingMessageType.CandidateRemoval:
                    var candidates = (ICECandidateRemovalMessage)msg;
                    _signalingEvents.OnRemoteIceCandidatesRemoved(candidates.Candidates);
                    break;
                case SignalingMessageType.Offer:
                    if (!_initiator)
                    {
                        var sdp = (SessionDescriptionMessage)msg;
                        _signalingEvents.OnRemoteDescription(sdp.Description);
                    }
                    else
                    {
                        ReportError($"Received offer for call receiver : {message}");
                    }
                    break;
                case SignalingMessageType.Answer:
                    if (_initiator)
                    {
                        var sdp = (SessionDescriptionMessage)msg;
                        _signalingEvents.OnRemoteDescription(sdp.Description);
                    }
                    else
                    {
                        ReportError($"Received answer for call initiator: {message}");
                    }
                    break;
                case SignalingMessageType.Bye:
                    _signalingEvents.OnChannelClose();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnWebSocketError(string description)
        {
            ReportError($"WebSocket error: {description}");
        }

        // Connects to room - function runs on a local looper thread.
        private void ConnectToRoomInternal()
        {
            var connectionUrl = GetConnectionUrl(_connectionParameters);
            _logger.Debug(TAG, $"Connect to room: {connectionUrl}");

            State = ConnectionState.New;
            _wsClient = new WebSocketChannelClient(_executor, this, _logger);

            var roomParametersFetcher = new RoomParametersFetcher(connectionUrl, null, _logger);
            roomParametersFetcher.MakeRequest((parameters, description) =>
            {
                _executor.Execute(() =>
                {
                    if (description != null)
                    {
                        ReportError(description);
                        return;
                    }

                    SignalingParametersReady(parameters);
                });
            });
        }

        // Disconnect from room and send bye messages - runs on a local looper thread.
        private void DisconnectFromRoomInternal()
        {
            _logger.Debug(TAG, "Disconnect. Room state: " + State);
            if (State == ConnectionState.Connected)
            {
                _logger.Debug(TAG, "Closing room.");
                SendPostMessage(MessageType.Leave, _leaveUrl, null);
            }
            State = ConnectionState.Closed;
            _wsClient?.Disconnect(true);
            _executor.Release();
        }

        private void SendPostMessage(MessageType messageType, string url, string message)
        {
            var logInfo = url;

            if (message != null)
                logInfo += $". Message: {message}";

            _logger.Debug(TAG, $"C->GAE: {logInfo}");

            var httpConnection = new AsyncHttpURLConnection(MethodType.Post, url, message, ((response, errorMessage) =>
            {
                _executor.Execute(() =>
                {
                    if (errorMessage != null)
                    {
                        ReportError($"GAE POST error : {errorMessage}");
                        return;
                    }

                    if (messageType != MessageType.Message)
                        return;
                    try
                    {
                        var msg = JsonConvert.DeserializeObject<MessageResponse>(response);
                        if (msg.Type != MessageResultType.Success)
                        {
                            ReportError($"GAE POST error : {response}");
                        }
                    }
                    catch (JsonException e)
                    {
                        ReportError($"GAE POST JSON error: {e.Message}");
                    }
                });
            }));
            httpConnection.Send();
        }

        private void SignalingParametersReady(SignalingParameters signalingParameters)
        {
            _logger.Debug(TAG, "Room connection completed.");

            if (_connectionParameters.IsLoopback &&
                (!signalingParameters.IsInitiator || signalingParameters.OfferSdp != null))
            {
                ReportError("Loopback room is busy.");
                return;
            }

            if (!_connectionParameters.IsLoopback && !signalingParameters.IsInitiator &&
                signalingParameters.OfferSdp == null)
            {
                _logger.Warning(TAG, "No offer SDP in room response.");
            }

            _initiator = signalingParameters.IsInitiator;
            _messageUrl = GetMessageUrl(_connectionParameters, signalingParameters);
            _leaveUrl = GetLeaveUrl(_connectionParameters, signalingParameters);

            _logger.Debug(TAG, $"Message URL: {_messageUrl}");
            _logger.Debug(TAG, $"Leave URL: {_leaveUrl}");

            State = ConnectionState.Connected;

            _signalingEvents.OnChannelConnected(signalingParameters);

            _wsClient.Connect(signalingParameters.WssUrl, signalingParameters.WssPostUrl);
            _wsClient.Register(_connectionParameters.RoomId, signalingParameters.ClientId);
        }

        private void ReportError(string errorMessage)
        {
            _logger.Error(TAG, errorMessage);
            _executor.Execute(() =>
            {
                if (State == ConnectionState.Error)
                    return;
                State = ConnectionState.Error;
                _signalingEvents.OnChannelError(errorMessage);
            });
        }

        private static string GetConnectionUrl(RoomConnectionParameters connectionParameters)
        {
            return connectionParameters.RoomUrl + "/" + ROOM_JOIN + "/" + connectionParameters.RoomId +
                   GetQueryString(connectionParameters);
        }

        private static string GetMessageUrl(RoomConnectionParameters connectionParameters,
            SignalingParameters signalingParameters)
        {
            return connectionParameters.RoomUrl + "/" + ROOM_MESSAGE + "/" + connectionParameters.RoomId + "/" +
                   signalingParameters.ClientId + GetQueryString(connectionParameters);
        }

        private static string GetLeaveUrl(RoomConnectionParameters connectionParameters,
            SignalingParameters signalingParameters)
        {
            return connectionParameters.RoomUrl + "/" + ROOM_LEAVE + "/" + connectionParameters.RoomId + "/" +
                   signalingParameters.ClientId + GetQueryString(connectionParameters);
        }

        private static string GetQueryString(RoomConnectionParameters roomConnectionParameters)
        {
            return roomConnectionParameters.UrlParameters != null ? $"?{roomConnectionParameters.UrlParameters}" : "";
        }

      
    }
}