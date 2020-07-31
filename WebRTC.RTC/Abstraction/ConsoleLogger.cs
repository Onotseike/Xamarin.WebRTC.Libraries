// onotseike@hotmail.comPaula Aliu
using System;

namespace WebRTC.RTC.Abstraction
{
    public class ConsoleLogger : ILogger
    {
        #region Implementations of ILogger

        public LogLevel LogLevel { get; set; }

        public void Debug(string tag, string message)
        {
            if (LogLevel > LogLevel.Debug)
                return;
            LogRecord(tag, message, "DEBUG");
        }

        public void Info(string tag, string message)
        {
            if (LogLevel > LogLevel.Info)
                return;
            LogRecord(tag, message, "INFO");
        }

        public void Error(string tag, string message)
        {
            if (LogLevel > LogLevel.Error)
                return;
            LogRecord(tag, message, "ERROR");
        }

        public void Warning(string tag, string message)
        {
            if (LogLevel > LogLevel.Warning)
                return;
            LogRecord(tag, message, "WARNING");
        }

        public void Error(string tag, string message, Exception ex)
        {
            if (LogLevel > LogLevel.Error)
                return;
            LogRecord(tag, message, "ERROR", ex);
        }

        #endregion

        #region Helper Method(s)

        private void LogRecord(string tag, string message, string logType, Exception exc = null)
        {
            string rec;
            if (exc == null)
                rec = $"{DateTime.UtcNow} ({tag}) {logType} - {message}";
            else
                rec =
                    $"{DateTime.UtcNow} ({tag}) {logType} {message}. EXCEPTION: {exc.Message}. STACK TRACE: {exc.StackTrace ?? ""}.";

            System.Diagnostics.Debug.WriteLine(rec);
        }

        #endregion

    }

}