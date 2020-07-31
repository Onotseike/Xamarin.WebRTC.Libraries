// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.RTC.Abstraction;

namespace WebRTC.RTC
{
    public class RoomConnectionParameters : IConnectionParameters
    {
        public string RoomUrl { get; set; }
        public string RoomId { get; set; }

        public bool IsLoopback { get; set; }
        public string UrlParameters { get; set; }
    }
}
