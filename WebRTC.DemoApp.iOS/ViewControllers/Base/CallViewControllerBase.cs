// onotseike@hotmail.comPaula Aliu
using System;

using AVFoundation;

using CoreGraphics;

using Foundation;

using UIKit;

using WebRTC.DemoApp.iOS.Interfaces;
using WebRTC.DemoApp.iOS.Views;
using WebRTC.Interfaces;
using WebRTC.iOS;
using WebRTC.iOS.Binding;
using WebRTC.RTC.Abstraction;

using Xamarin.Essentials;

namespace WebRTC.DemoApp.iOS.ViewControllers.Base
{
    public abstract class CallViewControllerBase : UIViewController, IVideoCallViewDelegate, IRTCEngineEvents, IRTCAudioSessionDelegate
    {

        #region Properties & Variables

        private AVAudioSessionPortOverride portOverride;

        private VideoCallView videoCallView;

        private VideoRendererProxy localVideoRenderer;
        private VideoRendererProxy remoteVideoRenderer;

        private FileCapturerController fileCapturerController;

        public IVideoCallViewControllerDelegate Delegate { get; set; }

        public bool isSimulator => ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR;

        #endregion

        #region Constructor(s)



        #endregion

        #region Overrides of UIViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            localVideoRenderer = new VideoRendererProxy();
            remoteVideoRenderer = new VideoRendererProxy();

            localVideoRenderer.Renderer = videoCallView.LocalVideoRender;
            remoteVideoRenderer.Renderer = videoCallView.RemoteVideoRender;
        }

        public override void LoadView()
        {
            base.LoadView();

            videoCallView = new VideoCallView(CGRect.Empty, !isSimulator);
            videoCallView.Delegate = this;

            View = videoCallView;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.All;
        }

        #endregion

        #region Implementation of IVideoCallViewDelegate

        public void DidChangeRoute(VideoCallView view)
        {
            var @override = AVAudioSessionPortOverride.None;
            if (portOverride == AVAudioSessionPortOverride.None)
            {
                @override = AVAudioSessionPortOverride.Speaker;
            }

            RTCDispatcher.DispatchAsyncOnType(RTCDispatcherQueueType.AudioSession, () =>
            {
                var session = RTCAudioSession.SharedInstance;
                session.LockForConfiguration();
                session.OverrideOutputAudioPort(@override, out NSError error);

                if (error == null)
                {
                    portOverride = @override;
                }
                else
                {
                    Console.WriteLine("Error overriding output port:{0}", error.LocalizedDescription);
                }
                session.UnlockForConfiguration();
            });
        }

        public void DidEnableStats(VideoCallView view)
        {

        }

        public void DidHangUp(VideoCallView view)
        {
            Disconnect();
        }

        public void DidSwitchCamera(VideoCallView view)
        {
            if (fileCapturerController != null)
            {
                fileCapturerController.Toggle();
                return;
            }
            SwitchCamera();
        }

        public void OnDisconnect(DisconnectType disconnectType)
        {
            Disconnect();
        }

        public void OnError(string description)
        {
            var alertDialog = UIAlertController.Create("Error", description, UIAlertControllerStyle.Alert);
            alertDialog.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, (s) => Disconnect()));
            PresentViewController(alertDialog, true, null);
        }

        public void OnPeerFactoryCreated(IPeerConnectionFactory peerConnectionFactory)
        {

        }

        public async void ReadyToStart()
        {
            var permission = await Permissions.RequestAsync<Permissions.Camera>();
            if (permission == PermissionStatus.Granted)
            {
                StartVideoCallInternal(localVideoRenderer, remoteVideoRenderer);
            }
            else
            {

            }
        }

        [Export("audioSession:didDetectPlayoutGlitch:")]
        public void AudioSession(RTCAudioSession audioSession, long totalNumberOfGlitches)
        {
            Console.WriteLine("Audio session detected glitch, total:{0}", totalNumberOfGlitches);
        }

        #endregion

        #region Implementations of IRTCEngineEvents

        public IVideoCapturer CreateVideoCapturer(IPeerConnectionFactory factory, IVideoSource videoSource)
        {
            if (!isSimulator)
            {
                return factory.CreateCameraCapturer(videoSource, true);
            }
            fileCapturerController = new FileCapturerController(videoSource);
            return fileCapturerController;
        }

        #endregion


        #region Helper Functions

        protected abstract void Disconnect();
        protected abstract void SwitchCamera();
        protected abstract void StartVideoCall(IVideoRenderer localRenderer, IVideoRenderer remoteRenderer);

        private void StartVideoCallInternal(IVideoRenderer localRenderer, IVideoRenderer remoteRenderer)
        {

            var session = RTCAudioSession.SharedInstance;

            session.UseManualAudio = true;
            session.IsAudioEnabled = false;


            StartVideoCall(localRenderer, remoteRenderer);
        }


        #endregion

    }


    public abstract class CallViewControllerBase<TConnectionParameters, TSignalParameters, TController> : CallViewControllerBase
        where TSignalParameters : ISignalingParameters
        where TConnectionParameters : IConnectionParameters
        where TController : RTCControllerBase<TConnectionParameters, TSignalParameters>
    {
        #region Properties & Variables

        private TController controller;

        #endregion

        #region Overrides of UIViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //controller = CreateController();
            //Connect(controller);
        }

        #endregion

        #region Overrides of  CallViewControllerBase Abstract methods

        protected override void Disconnect()
        {
            controller.Disconnect();
            Delegate?.DidFinish(this);
        }

        protected override void SwitchCamera()
        {
            controller.SwitchCamera();
        }

        protected override void StartVideoCall(IVideoRenderer localRenderer, IVideoRenderer remoteRenderer)
        {
            controller.StartVideoCall(localRenderer, remoteRenderer);
        }

        #endregion

        #region Helper Fuction(s)

        protected abstract TController CreateController();
        protected abstract void Connect(TController _controller);

        #endregion
    }

}
