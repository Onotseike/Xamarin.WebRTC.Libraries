// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Servers.Interfaces
{
    public interface IWebSocketChannelEvents
    {
        void OnWebSocketClose();
        void OnWebSocketMessage(string message);
        void OnWebSocketError(string errorMessage);
    }
}
