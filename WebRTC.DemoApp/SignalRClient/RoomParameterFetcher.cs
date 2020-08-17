// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.DemoApp.SignalRClient.Abstraction;
using WebRTC.RTC;

namespace WebRTC.DemoApp.SignalRClient
{
    public class RoomParameterFetcher
    {
        #region Properties & Variables

        private const string TAG = nameof(RoomParameterFetcher);
        private readonly string roomUrl;
        private readonly string roomMessage;

        private readonly ILogger logger;

        public delegate void RoomParameterFetcherCallback(SignalingParameters signalingParameters,
            string errorDescription);

        #endregion

        #region Constructor(s)

        public RoomParameterFetcher(string _roomUrl, string _roomMessage, ILogger _logger = null)
        {
            roomUrl = _roomUrl;
            roomMessage = _roomUrl;
            logger = _logger ?? new ConsoleLogger();
        }

        #endregion

        #region Main Function(s)

        public void MakeRequest(RoomParameterFetcherCallback _callBack)
        {

        }

        #endregion
    }
}
