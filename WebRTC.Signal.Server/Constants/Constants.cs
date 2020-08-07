// onotseike@hotmail.comPaula Aliu
using System;

using Newtonsoft.Json.Linq;

namespace WebRTC.Signal.Server.Constants
{
    /// <summary>
    /// Signal Server WebRTC related constants.
    /// </summary>
    public static class Constants
    {
        #region Properties

        public static string ServerURL { get; set; }
        public static JArray IceServers { get; set; }


        #endregion

        #region Variables

        private static int roomMemCacheExpiration = 60 * 60 * 24;
        private static int memCacheRetryLimit = 100;
        private static string LoopBackClientId = "LOOPBACK_CLIENT_ID";

        #endregion
    }
}
