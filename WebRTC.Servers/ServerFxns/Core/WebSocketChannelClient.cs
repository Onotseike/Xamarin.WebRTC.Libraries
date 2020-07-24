// onotseike@hotmail.comPaula Aliu
using System;

using Newtonsoft.Json;

using WebRTC.Servers.Enums;
using WebRTC.Servers.Interfaces;
using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class WebSocketChannelClient : WebSocketChannelClientBase
    {
        private const string TAG = nameof(WebSocketChannelClient);
        private string postServerUrl;

        private string roomId;
        private string clientId;

        public WebSocketChannelClient(IExecutor executor, IWebSocketChannelEvents events, ILogger _logger = null) : base(executor, events, _logger)
        {
        }

        public override void Connect(string wsUrl, string protocol)
        {
            base.Connect(wsUrl, null);
            postServerUrl = protocol;
        }

        public void Register(string _roomId, string _clientId)
        {
            CheckIfCalledOnValidThread("WebSocketChannelClient.Register ");
            roomId = _roomId;
            clientId = _clientId;

            if (State != WebSocketConnectionState.Connected)
            {
                Logger.Debug(TAG, $"WebSocket Register() in state {State}");
                return;
            }

            var registerMessage = new
            {
                cmd = "register",
                roomid = roomId,
                clientid = clientId
            };

            var json = JsonConvert.SerializeObject(registerMessage);

            Logger.Debug(TAG, $"C --> WSS: {json}");
            WebSocketConnection.Send(json);
            State = WebSocketConnectionState.Registered;

            foreach (var sendMessage in WebSocketQueue)
            {
                Send(sendMessage);
            }

            WebSocketQueue.Clear();
        }

        protected override string GetRegisterMessage(string message)
        {
            return JsonConvert.SerializeObject(new { cmd = "send", msg = message });
        }

        protected override void OnConnectionOpen()
        {
            if (clientId != null && roomId != null)
            {
                Register(roomId, clientId);
            }
        }

        protected override void SendByeMessage()
        {
            Send(SignalingMessage.CreateByeJson());
            SendWSSMessage(HttpMethodType.Delete, "");
        }

        private void SendWSSMessage(HttpMethodType methodType, string message)
        {
            var postUrl = $"{postServerUrl} / {roomId} / {clientId}";
            Logger.Debug(TAG, $"WS {methodType} : {postUrl} : {message}");

            var httpConnection = new AsyncHttpURLConnection(methodType, postUrl, message, (response, errorMsg) =>
            {
                if (errorMsg != null)
                {
                    ReportError($"WebSocket {methodType} error: {errorMsg}");
                }
            });

            httpConnection.Send();
        }
    }
}
