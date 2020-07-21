// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Interfaces;
using WebRTC.Servers.Enums;
using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.Models
{
    public class RTCEventLog
    {
        private const string TAG = nameof(RTCEventLog);
        private static int OutputFilMaxBytes = 10_000_000;

        private readonly IPeerConnection peerConnection;
        private readonly ILogger logger;
        private readonly string file;

        private RTCEventLogState state;

        public RTCEventLog(IPeerConnection _peerConnection, string _file, ILogger _logger = null)
        {
            peerConnection = _peerConnection ?? throw new ArgumentNullException(nameof(_peerConnection), "The Peer Connection is null");
            logger = _logger ?? new ConsoleLogger();
            file = _file;
        }

        public void Start()
        {
            if (state == RTCEventLogState.Started)
            {
                logger.Debug(TAG, "RTCEventLog has already started.");
                return;
            }
            var isSuccessful = peerConnection.StartRtcEventLog(file, OutputFilMaxBytes);
            if (!isSuccessful)
            {
                logger.Error(TAG, "Failed to start RTC Event Log.");
            }

            state = RTCEventLogState.Started;
            logger.Debug(TAG, "RTCEventLog started.");
        }

        public void Stop()
        {
            if (state != RTCEventLogState.Started)
            {
                logger.Error(TAG, "RTCEventLog was not started.");
                return;
            }
            peerConnection.StopRtcEventLog();
            state = RTCEventLogState.Stopped;
            logger.Debug(TAG, "RTCEventLog Stopped");
        }
    }
}
