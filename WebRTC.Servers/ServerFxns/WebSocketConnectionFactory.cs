// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.ServerFxns
{
    public static class WebSocketConnectionFactory
    {
        public static Func<IWebSocketConnection> Factory { get; set; }
        public static IWebSocketConnection CreateWebSocketConnection() => Factory();
    }
}
