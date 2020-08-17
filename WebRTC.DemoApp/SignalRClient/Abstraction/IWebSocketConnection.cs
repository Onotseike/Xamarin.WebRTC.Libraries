// onotseike@hotmail.com
//Paula Aliu


using System;
namespace WebRTC.DemoApp.SignalRClient.Abstraction
{
    public interface IWebSocketConnection : IDisposable
    {
        #region Events(s)

        event EventHandler OnOpened;
        event EventHandler<(int code, string reason)> OnClosed;
        event EventHandler<Exception> OnError;
        event EventHandler<string> OnMessage;

        #endregion

        #region Method(s)

        void Open(string _url, string _protocol = null, string _authToken = null);
        void Close();
        void Send(string _message);

        #endregion
    }
}
