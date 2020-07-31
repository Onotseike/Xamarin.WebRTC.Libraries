// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Threading;

namespace WebRTC.RTC.Abstraction
{

    #region WebSocketConnectionState

    public enum WebSocketConnectionState
    {
        New,
        Connected,
        Registered,
        Closed,
        Error
    }

    #endregion

    public interface IWebSocketChannelEvents
    {

        #region Method(s)

        void OnWebSocketClose();
        void OnWebSocketMessage(string _message);
        void OnWebSocketError(string _description);

        #endregion

    }



    public abstract class WebSocketChannelClientBase
    {
        private const string TAG = nameof(WebSocketChannelClientBase);

        private const int CloseTimeout = 1000;


        private readonly IExecutor _executor;
        private readonly IWebSocketChannelEvents _events;

        private ManualResetEvent _mre;
        private string _wsUrl;

        protected WebSocketChannelClientBase(IExecutor executor, IWebSocketChannelEvents events, ILogger logger = null)
        {
            _executor = executor;
            _events = events;
            WebSocketConnection = WebSocketConnectionFactory.CreateWebSocketConnection();
            Logger = logger ?? new ConsoleLogger();

            State = WebSocketConnectionState.New;
        }

        protected ILogger Logger { get; }

        protected IWebSocketConnection WebSocketConnection { get; }

        protected readonly List<string> WsSendQueue = new List<string>();

        public WebSocketConnectionState State { get; protected set; }

        public virtual void Connect(string wsUrl, string protocol)
        {
            CheckIfCalledOnValidThread();
            WireEvents();

            _wsUrl = wsUrl;
            Logger.Debug(TAG, $"Connecting WebSocket to:{wsUrl}. Protocol :{protocol}");
            WebSocketConnection.Open(wsUrl, protocol);
        }

        public void Disconnect(bool waitForComplete)
        {
            CheckIfCalledOnValidThread();
            Logger.Debug(TAG, $"Disconnect WebSocket. State: {State}");
            if (State == WebSocketConnectionState.Registered)
            {
                // Send "bye" to WebSocket server.
                SendByeMessage();
                State = WebSocketConnectionState.Connected;
            }

            if (State == WebSocketConnectionState.Connected || State == WebSocketConnectionState.Error)
            {
                if (waitForComplete)
                    _mre = new ManualResetEvent(false);
                WebSocketConnection.Close();
                State = WebSocketConnectionState.Closed;

                if (waitForComplete)
                {
                    try
                    {
                        _mre.WaitOne(CloseTimeout);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(TAG, $"Wait error:{ex}");
                    }
                }
            }

            UnWireEvents();

            Logger.Debug(TAG, "Disconnecting WebSocket done.");
        }

        public virtual void Send(string message)
        {
            CheckIfCalledOnValidThread();
            switch (State)
            {
                case WebSocketConnectionState.New:
                case WebSocketConnectionState.Connected:
                    Logger.Debug(TAG, $"WS ACC: {message}");
                    WsSendQueue.Add(message);
                    break;
                case WebSocketConnectionState.Closed:
                case WebSocketConnectionState.Error:
                    Logger.Error(TAG, $"WebSocket send() in error or closed state: {message}");
                    break;
                case WebSocketConnectionState.Registered:
                    message = GetRegisterMessage(message);
                    Logger.Debug(TAG, $"C->WSS: {message}");
                    WebSocketConnection.Send(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual string GetRegisterMessage(string message) => message;


        protected abstract void SendByeMessage();

        protected abstract void OnConnectionOpen();


        protected virtual void OnMessageReceived(string message)
        {
            _events.OnWebSocketMessage(message);
        }


        protected void CheckIfCalledOnValidThread()
        {
            if (!_executor.IsCurrentExecutor)
                throw new InvalidOperationException("WebSocket method is not called on valid executor.");
        }

        protected void ReportError(string errorMessage)
        {
            Logger.Error(TAG, errorMessage);
            _executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Error)
                    return;
                State = WebSocketConnectionState.Error;
                _events.OnWebSocketError(errorMessage);
            });
        }

        private void WireEvents()
        {
            WebSocketConnection.OnOpened += WebSocketConnectionOnOnOpened;
            WebSocketConnection.OnClosed += WebSocketConnectionOnOnClosed;
            WebSocketConnection.OnError += WebSocketConnectionOnOnError;
            WebSocketConnection.OnMessage += WebSocketConnectionOnOnMessage;
        }

        private void UnWireEvents()
        {
            WebSocketConnection.OnOpened -= WebSocketConnectionOnOnOpened;
            WebSocketConnection.OnClosed -= WebSocketConnectionOnOnClosed;
            WebSocketConnection.OnError -= WebSocketConnectionOnOnError;
            WebSocketConnection.OnMessage -= WebSocketConnectionOnOnMessage;
        }

        private void WebSocketConnectionOnOnOpened(object sender, EventArgs e)
        {
            Logger.Debug(TAG, $"WebSocket connection opened to:{_wsUrl}");
            _executor.Execute(() =>
            {
                State = WebSocketConnectionState.Connected;
                OnConnectionOpen();
            });
        }

        private void WebSocketConnectionOnOnMessage(object sender, string e)
        {
            Logger.Debug(TAG, $"WSS->C: {e}");
            _executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Connected || State == WebSocketConnectionState.Registered)
                    OnMessageReceived(e);
            });
        }

        private void WebSocketConnectionOnOnError(object sender, Exception e)
        {
            ReportError(e.Message);
        }

        private void WebSocketConnectionOnOnClosed(object sender, (int code, string reason) e)
        {
            Logger.Debug(TAG, $"WebSocket connection closed. Code: {e.code}. Reason: {e.reason}. State: {State}");
            _mre?.Set();
            _executor.Execute(() =>
            {
                if (State == WebSocketConnectionState.Closed)
                    return;
                State = WebSocketConnectionState.Closed;
                _events.OnWebSocketClose();
            });
        }
    }
}
