// onotseike@hotmail.comPaula Aliu
using System;

using Xamarin.Forms;

namespace WebRTC.DemoApp
{
    public class CallPage : ContentPage
    {
        public string RoomId { get; set; }

        public CallPage(string roomId)
        {
            RoomId = roomId;
        }
    }
}

