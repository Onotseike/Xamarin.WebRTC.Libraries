// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using WebRTC.RTC.Abstraction;

namespace WebRTC.RTC
{
    public class WebSocketChannelClient : WebSocketChannelClientBase
    {
        private const string TAG = nameof(WebSocketChannelClient);

        private string _postServerUrl;

        private string _roomId;
        private string _clientId;


        public WebSocketChannelClient(IExecutor executor, IWebSocketChannelEvents events, ILogger logger = null) : base(executor, events, logger)
        {
        }

        public override void Connect(string wsUrl, string postUrl)
        {
            base.Connect(wsUrl, null);
            _postServerUrl = postUrl;
        }

        public void Register(string roomId, string clientId)
        {
            CheckIfCalledOnValidThread();
            _roomId = roomId;
            _clientId = clientId;

            if (State != WebSocketConnectionState.Connected)
            {
                Logger.Debug(TAG, $"WebSocket register() in state {State}");
                return;
            }

            var registerMessage = new
            {
                cmd = "register",
                roomid = roomId,
                clientid = clientId
            };

            var json = JsonConvert.SerializeObject(registerMessage);


            Logger.Debug(TAG, $"C->WSS: {json}");

            WebSocketConnection.Send(json);

            State = WebSocketConnectionState.Registered;

            foreach (var sendMessage in WsSendQueue)
            {
                Send(sendMessage);
            }

            WsSendQueue.Clear();
        }

        protected override string GetRegisterMessage(string message)
        {
            return JsonConvert.SerializeObject(new
            {
                cmd = "send",
                msg = message
            });
        }

        protected override void SendByeMessage()
        {
            Send(SignalingMessage.CreateByeJson());
            SendWSSMessage(MethodType.Delete, "");
        }

        protected override void OnConnectionOpen()
        {
            if (_clientId != null && _roomId != null)
            {
                Register(_roomId, _clientId);
            }
        }

        protected override void OnMessageReceived(string message)
        {

            var webSocketMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);

            if (webSocketMessage.ContainsKey("error"))
            {
                var error = webSocketMessage["error"];
                if (!string.IsNullOrEmpty(error))
                {
                    ReportError($"WebSocket error message: {error}");
                    return;
                }
            }

            if (!webSocketMessage.TryGetValue("msg", out string msg) || string.IsNullOrEmpty(msg))
            {
                ReportError($"Unexpected WebSocket message: {message}");
                return;
            }


            base.OnMessageReceived(msg);
        }

        private void SendWSSMessage(MethodType method, string message)
        {
            var postUrl = $"{_postServerUrl}/{_roomId}/{_clientId}";
            Logger.Debug(TAG, $"WS {method} : {postUrl} : {message}");

            var httpConnection = new AsyncHttpURLConnection(method, postUrl, message, (response, errorMessage) =>
            {
                if (errorMessage != null)
                    ReportError($"WS {method} error: {errorMessage}");
            });

            httpConnection.Send();
        }
    }
}
