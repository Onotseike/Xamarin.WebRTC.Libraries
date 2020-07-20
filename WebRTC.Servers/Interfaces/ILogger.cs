// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.Servers.Interfaces
{
    public interface ILogger
    {
        void Debug(string tag, string message);

        void Info(string tag, string message);

        void Error(string tag, string message);
        void Error(string tag, string message, Exception exception);

        void Warning(string tag, string message);
    }
}
