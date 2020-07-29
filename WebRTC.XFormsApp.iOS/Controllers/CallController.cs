// onotseike@hotmail.comPaula Aliu
using System;

using CoreGraphics;

using UIKit;

using WebRTC.XFormsApp.iOS.Interfaces;
using WebRTC.XFormsApp.iOS.ViewControllerBases;
using WebRTC.XFormsApp.iOS.Views;

namespace WebRTC.XFormsApp.iOS.Controllers
{
    public class CallController : CallControllerBase
    {
        #region Properties & Variables

        private readonly string roomID;

        #endregion

        #region Constructor(s)

        public CallController(string _roomId)
        {
            roomID = _roomId;
        }

        #endregion


        #region Overrides from ControllerBase and it's Parent(s)

        public override void LoadView()
        {
            base.LoadView();
            Title = "Call Page";

            View = new UIMainView();

            CommenceCall();
        }

        #endregion

        #region Helper Function(s)

        public void CommenceCall()
        {

            var videoCallViewController = new VideoCallViewController(roomID)
            {
                Delegate = this
            };

            videoCallViewController.ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve;
            videoCallViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;

            PresentViewController(videoCallViewController, true, null);
        }

        #endregion

    }
}
