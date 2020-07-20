// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.Enums;
using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.Models
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel LogLevel { get; set; }

        public void Debug(string tag, string message)
        {
            if (LogLevel > LogLevel.Debug) return;

            LogRecord(tag, message, "DEBUG");

        }

        public void Error(string tag, string message)
        {
            if (LogLevel > LogLevel.Error) return;
            LogRecord(tag, message, "ERROR");
        }

        public void Error(string tag, string message, Exception exception)
        {
            if (LogLevel > LogLevel.Error) return;
            LogRecord(tag, message, "ERROR", exception);
        }

        public void Info(string tag, string message)
        {
            if (LogLevel > LogLevel.Info) return;
            LogRecord(tag, message, "INFO");
        }

        public void Warning(string tag, string message)
        {
            if (LogLevel > LogLevel.Warning) return;
            LogRecord(tag, message, "WARNINING");
        }

        #region Helper Functions

        private void LogRecord(string tag, string message, string logType, Exception exception = null)
        {
            string newRecord;
            if (exception == null)
            {
                newRecord = $"{DateTime.UtcNow} ({tag})  {logType} --->  {message}";
            }
            else
            {
                newRecord = $"{DateTime.UtcNow} ({tag})  {logType} ---> EXCEPTION:  {exception.Message}. STACK TRACE: {exception.StackTrace ?? "" } CUSTOM MESSAGE: {message}";
            }

            System.Diagnostics.Debug.WriteLine(newRecord);
        }


        #endregion
    }
}
