// onotseike@hotmail.comPaula Aliu
using System;

using WebRTC.XFormsApp.iOS.Views;

namespace WebRTC.XFormsApp.iOS.Interfaces
{
    public interface IVideoCallViewDelegate
    {
        void DidSwitchCamera(VideoCallView view);
        void DidChangeRoute(VideoCallView view);
        void DidHangUp(VideoCallView view);
        void DidEnableStats(VideoCallView view);
    }
}
