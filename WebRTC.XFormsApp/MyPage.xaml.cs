// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WebRTC.XFormsApp
{
    public partial class MyPage : ContentPage
    {
        public MyPage(string roomId)
        {
            InitializeComponent();
            callView.RoomID = roomId;
        }
    }
}
