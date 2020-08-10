// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

namespace WebRTC.Signal.Server.Models
{
    public class CallOffer
    {
        #region Properties

        public Client Initiator { get; set; }
        public List<Client> Participants { get; set; } = new List<Client>();

        #endregion
    }
}
