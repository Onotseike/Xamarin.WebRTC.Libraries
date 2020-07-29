// onotseike@hotmail.comPaula Aliu
using System;

using AVFoundation;

using Foundation;

using UIKit;

using WebRTC.iOS.Binding;
using WebRTC.XFormsApp.iOS.Interfaces;
using WebRTC.XFormsApp.iOS.Views;

namespace WebRTC.XFormsApp.iOS.ViewControllerBases
{
    public abstract class CallControllerBase : UIViewController, IRTCAudioSessionDelegate, IVideoCallViewControllerDelegate
    {
        #region Constructor(s)

        public CallControllerBase()
        {

        }

        public CallControllerBase(IntPtr _handle) : base(_handle)
        {

        }

        #endregion

        #region Overrides of the UIViewController

        public override void ViewDidLoad()
        {
            base.ViewDidLoad(); // Call the base ViewDidLoad()

            // Set Up WebRTCConfiguration
            // Default Audio Output Default Speaker
            var webRTCConfig = new RTCAudioSessionConfiguration();
            webRTCConfig.CategoryOptions |= AVAudioSessionCategoryOptions.DefaultToSpeaker;
            RTCAudioSessionConfiguration.SetWebRTCConfiguration(webRTCConfig);

            var audioSession = RTCAudioSession.SharedInstance;
            audioSession.AddDelegate(this);

            ConfigureAudioSession();
        }

        #endregion

        #region Implementations of IVideoCallViewControllerDelegate

        public void DidFinish(CallViewControllerBase _viewController)
        {
            if (!_viewController.IsBeingDismissed)
            {
                Console.WriteLine("Dismissing VC");
                _viewController.DismissViewController(true, OnDismissViewController);
            }
            var session = RTCAudioSession.SharedInstance;
            session.IsAudioEnabled = false;
        }

        #endregion

        #region Overrides of IRTCAudioSessionDelegate

        [Export("audioSessionDidStartPlayOrRecord:")]
        private void AudioSessionDidStartPlayOrRecordInternal(RTCAudioSession _audioSession)
        {
            // Stop any playback on main queue then configure WebRTC
            RTCDispatcher.DispatchAsyncOnType(RTCDispatcherQueueType.Main, () =>
           {
               Console.WriteLine("Setting isAudioEnabled to YES");
               _audioSession.IsAudioEnabled = true;
           });
        }

        [Export("audioSessionDidStopPlayOrRecord:")]
        private void AudioSessionDidStopePlayOrRecordInternal(RTCAudioSession _audioSession)
        {
            // WebRTC is Done with the audio Session. Restart playback
            RTCDispatcher.DispatchAsyncOnType(RTCDispatcherQueueType.Main, () =>
           {
               AudioSessionDidStopePlayOrRecord(_audioSession);
           });
        }

        #endregion

        #region Helper Functions

        public void DidFinish(CallControllerBase _viewController)
        {
            if (!_viewController.IsBeingDismissed)
            {
                Console.WriteLine("Dismissing View Controller");
                _viewController.DismissViewController(true, OnDismissViewController);
            }

            var audioSession = RTCAudioSession.SharedInstance;
            audioSession.IsAudioEnabled = false;
        }

        private void OnDismissViewController()
        {

        }

        private void AudioSessionDidStopePlayOrRecord(RTCAudioSession _audioSession)
        {
            Console.WriteLine("AudioSessionDidStopPlayOrRecord");
            ConfigureAudioSession();
        }

        protected virtual void AudioSessionDidStartPlayOrRecord(RTCAudioSession _audioSession)
        {
            Console.WriteLine("Setting isAudioEnabled to YES.");
            _audioSession.IsAudioEnabled = true;
        }

        private void ConfigureAudioSession()
        {
            var configuration = new RTCAudioSessionConfiguration();
            configuration.Category = AVAudioSession.CategoryAmbient;
            configuration.CategoryOptions = AVAudioSessionCategoryOptions.DuckOthers;
            configuration.Mode = AVAudioSession.ModeDefault;

            var session = RTCAudioSession.SharedInstance;
            session.LockForConfiguration();

            bool hasSucceeded;
            NSError error;
            if (session.IsActive)
            {
                hasSucceeded = session.SetConfiguration(configuration, out error);
            }
            else
            {
                hasSucceeded = session.SetConfiguration(configuration, true, out error);
            }

            if (!hasSucceeded)
            {
                Console.WriteLine($"Error Setting configuration: {0}", error.LocalizedDescription);
            }

            session.UnlockForConfiguration();
            FinishConfigurationAudioSessions();
        }

        protected virtual void FinishConfigurationAudioSessions()
        {

        }



        #endregion
    }
}
