// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.SignalR;

using WebRTC.Signal.Server.Models;

namespace WebRTC.Signal.Server.Hubs
{
    public class WebRTCHub : Hub
    {
        #region Properties

        public List<Room> Rooms { get; set; }
        public List<Client> HubClients { get; set; }
        public List<CallOffer> CallOffers { get; set; }

        #endregion

        public WebRTCHub()
        {
            Rooms = new List<Room>();
            HubClients = new List<Client>();
            CallOffers = new List<CallOffer>();
        }

        #region Function(s)

        public void JoinHub()
        { }


        #endregion
    }
}
