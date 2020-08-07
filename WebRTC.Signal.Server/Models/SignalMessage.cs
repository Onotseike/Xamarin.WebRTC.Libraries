// onotseike@hotmail.comPaula Aliu
using System;

namespace WebRTC.Signal.Server.Models
{
    public class SignalMessage
    {
        #region Properties

        public Guid MesssageId { get; set; }
        public string Data { get; set; }

        #endregion
    }
}