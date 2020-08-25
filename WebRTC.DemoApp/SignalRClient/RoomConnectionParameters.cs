// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.DemoApp.SignalRClient.Abstraction;

namespace WebRTC.DemoApp.SignalRClient
{
    public class RoomConnectionParameters : IConnectionParameters
    {
        public string RoomUrl { get; set; }
        public string RoomId { get; set; }
        public int RoomOccupancy { get; set; }
        public bool IsInitator { get; set; }
        public bool IsLoopback { get; set; }
        public string UrlParameters { get; set; }
    }
}
