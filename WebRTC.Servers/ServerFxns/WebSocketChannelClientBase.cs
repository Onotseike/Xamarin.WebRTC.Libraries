// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Threading;

using WebRTC.Servers.Enums;
using WebRTC.Servers.Interfaces;
using WebRTC.Servers.Models;

namespace WebRTC.Servers.ServerFxns
{
    public abstract class WebSocketChannelClientBase
    {
        private const string TAG = nameof(WebSocketChannelClientBase);
        private const int CloseTimeout = 1000;

        private readonly IExecutor executor;
        private readonly IWebSocketChannelEvents webSocketChannelEvents;

        private ManualResetEvent manualResetEvent;
        private string webSocketURL;

        protected ILogger Logger { get; }
        protected IWebSocketConnection WebSocketConnection { get; }
        protected readonly List<string> WebSocketQueue = new List<string>();

        public WebSocketConnectionState State { get; protected set; }

        public WebSocketChannelClientBase(IExecutor _executor, IWebSocketChannelEvents _events, ILogger _logger = null)
        {
            executor = _executor;
            webSocketChannelEvents = _events;
            WebSocketConnection = WebSocketConnectionFactory.CreateWebSocketConnection();
            Logger = _logger ?? new ConsoleLogger();
        }

        public virtual void Connect(string wsUrl, string protocol)
        {
            CheckIfCalledOnValidThread("Connect");
            WireEvents();

            webSocketURL = wsUrl;
            Logger.Debug(TAG, $"Connecting WebSocket to: {webSocketURL}. Protocol: {protocol}");
            WebSocketConnection.Open(webSocketURL, protocol);
        }

        public void Disconnect(bool waitForComplete)
        {
            CheckIfCalledOnValidThread("Disconnect");
            Logger.Debug(TAG, $"Disconnect WebSocket. State: {State}");
            if (State == WebSocketConnectionState.Registered)
            {
                SendByeMessage();
                State = WebSocketConnectionState.Connected;
            }

            if (State == WebSocketConnectionState.Connected || State == WebSocketConnectionState.Error)
            {
                if (waitForComplete)
                {
                    manualResetEvent = new ManualResetEvent(false);
                }
                WebSocketConnection.Close();
                State = WebSocketConnectionState.Closed;

                if (waitForComplete)
                {
                    try
                    {
                        manualResetEvent.WaitOne(CloseTimeout);
                    }
                    catch (Exception ex)
                    {
                        Logger.Debug(TAG, "Disconnecting WebSocket Done. ");
                    }
                }
            }

        }

        public virtual void Send(string message)
        {
            CheckIfCalledOnValidThread("Send");
            switch (State)
            {
                case WebSocketConnectionState.New:
                case WebSocketConnectionState.Connected:
                    Logger.Debug(TAG, $"WebSocket ACC: {message}");
                    WebSocketQueue.Add(message);
                    break;
                case WebSocketConnectionState.Registered:
                    message = GetRegisterMessage(message);
                    Logger.Debug(TAG, $"Connection -> WebSocketS: {message}");
                    WebSocketConnection.Send(message);
                    break;
                case WebSocketConnectionState.Closed:
                case WebSocketConnectionState.Error:
                    Logger.Error(TAG, $"WebSocket Send() in error or closed state: {message}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }



        #region Helper Functions(Protected & Private)

        protected void CheckIfCalledOnValidThread(string method)
        {
            if (!executor.IsCurrentExecutor)
            {
                throw new InvalidOperationException($"WEBSOCKET  {method} method is not called on a valid executor.");
            }
        }

        private void WireEvents()
        {
            WebSocketConnection.OnOpened += WebSocketConnectionOnOpened;
            WebSocketConnection.OnClosed += WebSocketConnectionOnClosed;
            WebSocketConnection.OnError += WebSocketConnectionOnError;
            WebSocketConnection.OnMessage += WebSocketConnectionOnMessage;
        }

        private void UnWireEvents()
        {
            WebSocketConnection.OnOpened -= WebSocketConnectionOnOpened;
            WebSocketConnection.OnClosed -= WebSocketConnectionOnClosed;
            WebSocketConnection.OnError -= WebSocketConnectionOnError;
            WebSocketConnection.OnMessage -= WebSocketConnectionOnMessage;
        }

        protected void ReportError(string errorMessage)
        {
            Logger.Error(TAG, errorMessage);
            executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Error) return;

                State = WebSocketConnectionState.Error;
                webSocketChannelEvents.OnWebSocketError(errorMessage);
            });
        }

        protected virtual void OnMessageRecieved(string message) => webSocketChannelEvents.OnWebSocketMessage(message);

        protected abstract void OnConnectionOpen();

        protected virtual string GetRegisterMessage(string message) => message;

        protected abstract void SendByeMessage();

        #endregion


        #region Event Handlers

        private void WebSocketConnectionOnMessage(object sender, string e)
        {
            Logger.Debug(TAG, $"WebSocket Connection Message to: {e}");
            executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Connected || State == WebSocketConnectionState.Registered) OnMessageRecieved(e);

            });
        }

        private void WebSocketConnectionOnError(object sender, Exception e)
        {
            ReportError(e.Message);
        }

        private void WebSocketConnectionOnClosed(object sender, (int code, string reason) e)
        {
            Logger.Debug(TAG, $"WebSocket Connection closed. Code: {e.code}. Reason: {e.reason}. State: {State}");
            manualResetEvent?.Set();
            executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Closed) return;
                State = WebSocketConnectionState.Closed;
                webSocketChannelEvents.OnWebSocketClose();
            });
        }

        private void WebSocketConnectionOnOpened(object sender, EventArgs e)
        {
            Logger.Debug(TAG, $"WebSocket Connection opened to: {webSocketURL}");
            executor.Execute(() =>
            {
                State = WebSocketConnectionState.Connected;
                OnConnectionOpen();
            });
        }


        #endregion
    }
}
