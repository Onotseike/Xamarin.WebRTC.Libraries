// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.ServerFxns
{
    public static class ConnectionFactory
    {
        public static Func<IConnectionHub> Factory { get; set; }
        public static IConnectionHub CreateConnectionHub() => Factory();
    }
}
