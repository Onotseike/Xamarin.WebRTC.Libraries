using System;
using System.ComponentModel;

using WebRTC.DemoApp.Helper;

using Xamarin.Forms;

namespace WebRTC.DemoApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void StartCall_Clicked(object sender, EventArgs e)
        {
            var roomId = RoomIdEntry.Text ?? GenerateRoom.GenerateRoomName();
            RoomIdEntry.Text = roomId;

            await Navigation.PushAsync(new CallPage(roomId));
        }
    }
}
