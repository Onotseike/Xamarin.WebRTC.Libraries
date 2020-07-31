// onotseike@hotmail.comPaula Aliu
using System;

using CoreGraphics;

using UIKit;

using WebRTC.DemoApp;
using WebRTC.DemoApp.iOS.Interfaces;
using WebRTC.DemoApp.iOS.Renderers;
using WebRTC.DemoApp.iOS.ViewControllers;
using WebRTC.DemoApp.iOS.ViewControllers.Base;
using WebRTC.DemoApp.iOS.Views;
using WebRTC.iOS.Binding;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CallPage), typeof(CallPageRenderer))]
namespace WebRTC.DemoApp.iOS.Renderers
{
    public class CallPageRenderer : PageRenderer, IVideoCallViewControllerDelegate
    {
        #region Properties & Variables

        private string RoomId { get; set; }

        #endregion

        #region Overrides from PageRenderer

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }


            var callPage = (CallPage)e.NewElement;
            RoomId = callPage.RoomId;

            if (NativeView != null)
            {

            }


        }

        public override void LoadView()
        {
            base.LoadView();
            //var callController = new CallController(RoomId, false);
            //PresentViewController(callController, true, null);
            //View.AddSubview(callController.View);
            var view = new BaseView(CGRect.FromLTRB(2, 2, 2, 2));
            view.BackgroundColor = UIColor.Brown;
            View = view;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion



        #region Override of IVideoCallViewControllerDelegate

        public void DidFinish(CallViewControllerBase _viewController)
        {
            if (!_viewController.IsBeingDismissed)
            {
                Console.WriteLine("Dismissing VC");
                _viewController.DismissViewController(true, OnDismissVideoController);
            }
            var session = RTCAudioSession.SharedInstance;
            session.IsAudioEnabled = false;
        }
        #endregion

        #region Helper Function(s)
        protected virtual void OnDismissVideoController()
        {

        }
        #endregion
    }
}
