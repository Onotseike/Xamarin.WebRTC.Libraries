using System.ComponentModel;

using WebRTC.Servers.ServerFxns.Core;

using Xamarin.Forms;

namespace WebRTC.XFormsApp
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

        void StartVoiceCall_Clicked(object sender, System.EventArgs e)
        {
            var roomId = RoomIdEntry.Text ?? GenerateRoom.GenerateRoomName();
            RoomIdEntry.Text = roomId;
            App.CallingService.StartVoiceCall(roomId);
        }

        void StartVideoCall_Clicked(object sender, System.EventArgs e)
        {
            var roomId = RoomIdEntry.Text ?? GenerateRoom.GenerateRoomName();
            RoomIdEntry.Text = roomId;
            App.CallingService.StartVideoCall(roomId);
        }
    }
}
