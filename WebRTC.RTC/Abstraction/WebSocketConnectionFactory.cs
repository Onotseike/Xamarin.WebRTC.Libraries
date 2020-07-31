// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.RTC.Abstraction
{
    public static class WebSocketConnectionFactory
    {
        public static Func<IWebSocketConnection> Factory { get; set; }
        public static IWebSocketConnection CreateWebSocketConnection() => Factory();
    }
}
