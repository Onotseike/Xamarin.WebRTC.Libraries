// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.Servers.Interfaces;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class RoomConnectionParameters : IConnectionParameters
    {
        public string RoomURL { get; set; }
        public string RoomID { get; set; }
        public bool IsLoopBack { get; set; }
        public string UrlParameters { get; set; }
    }
}
