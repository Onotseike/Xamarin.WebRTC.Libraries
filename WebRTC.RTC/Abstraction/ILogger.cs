// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.RTC.Abstraction
{
    #region LogLevel Enum

    public enum LogLevel
    {
        All = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4
    }

    #endregion


    public interface ILogger
    {
        #region Method(s)

        void Debug(string _tag, string _message);
        void Info(string _tag, string _message);
        void Error(string _tag, string _message);
        void Error(string _tag, string _message, Exception _exception);
        void Warning(string _tag, string _message);

        #endregion

    }
}
