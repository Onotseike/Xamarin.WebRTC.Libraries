﻿// onotseike@hotmail.comPaula Aliu
using System;
using System.Security.Cryptography;

using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

using Org.Webrtc;

using WebRTC.Android;
using WebRTC.Android.Extensions;
using WebRTC.DemoApp;
using WebRTC.DemoApp.Droid.Activities;
using WebRTC.DemoApp.Droid.Fragments;
using WebRTC.DemoApp.Droid.Renderers;
using WebRTC.Enums;
using WebRTC.Interfaces;
using WebRTC.RTC;
using WebRTC.RTC.Abstraction;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CallPage), typeof(CallPageRenderer))]
namespace WebRTC.DemoApp.Droid.Renderers
{
    public class CallPageRenderer : PageRenderer, ICallPageRenderer<RoomConnectionParameters, SignalingParameters, RTCController>
    {
        #region Properties and Variables

        private string RoomId { get; set; }
        private CallFragment CallFragment { get; set; }
        private BaseActivity CallPageActivity { get; set; }
        private View CallView { get; set; }

        private RTCController CallController { get; set; }

        private SurfaceViewRenderer FullScreenRenderer { get; set; }
        private SurfaceViewRenderer PipScreenRenderer { get; set; }

        private VideoRendererProxy LocalVideoRenderer { get; set; }
        private VideoRendererProxy RemoteVideoRenderer { get; set; }

        private bool IsSwappedFeed { get; set; }
        private bool CallControlFragmentVisible = true;
        private bool IsMicEnabled = true;

        #endregion

        #region Constructor(s)

        public CallPageRenderer(Context context) : base(context)
        {

        }

        #endregion

        #region Implementations of ICallPageRenderer


        public RTCController CreateController() => new RTCController(this);

        public void Connect(RTCController _controller, Intent _intent) => _controller.Connect(new RoomConnectionParameters
        {
            RoomId = RoomId,
            IsLoopback = false,
            RoomUrl = "https://appr.tc/"
        });

        public CallFragment CreateCallFragment(Intent _intent) => CallFragment.Create(RoomId, true, true);




        public void OnPeerFactoryCreated(IPeerConnectionFactory factory)
        {
            var androidFactory = (IPeerConnectionFactoryAndroid)factory.NativeObject;
            PipScreenRenderer.Init(androidFactory.EglBaseContext, null);
            FullScreenRenderer.Init(androidFactory.EglBaseContext, null);
        }

        public void OnDisconnect(DisconnectType disconnectType) => Disconnect();

        public WebRTC.Interfaces.IVideoCapturer CreateVideoCapturer(IPeerConnectionFactory factory, IVideoSource videoSource) =>
            factory.CreateCameraCapturer(videoSource, true);

        public async void ReadyToStart()
        {
            var permission = await Permissions.RequestAsync<Permissions.Camera>();
            if (permission == PermissionStatus.Granted)
            {
                CallController.StartVideoCall(LocalVideoRenderer, RemoteVideoRenderer);
            }
            else
            {
                Toast.MakeText(CallPageActivity, "No Video permission was granted.", ToastLength.Long).Show();
            }

        }

        public void OnError(string description)
        {
            if (CallController == null) return;

            new AlertDialog.Builder(CallPageActivity).SetTitle("Error").SetMessage(description).SetCancelable(false).SetNeutralButton("OK", (sender, args) =>
            {
                var dialog = (AlertDialog)sender;
                dialog.Cancel();
                Disconnect();
            }).Create().Show();
        }

        public void OnCallHangUp() => Disconnect();

        public void OnCameraSwitch() => CallController?.SwitchCamera();

        public void OnVideoScalingSwitch(ScalingType _scalingType) => FullScreenRenderer.SetScalingType(_scalingType.ToNative());

        public void OnCaptureFormatChange(int _width, int _height, int _framerate) => CallController?.ChangeCaptureFormat(_width, _height, _framerate);

        public bool OnToggleMic()
        {
            if (CallController == null) return IsMicEnabled;

            IsMicEnabled = !IsMicEnabled;
            CallController.SetAudioEnabled(IsMicEnabled);
            return IsMicEnabled;
        }


        #endregion

        #region Implementations of PageRenderer

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            var callPage = (CallPage)e.NewElement;
            RoomId = callPage.RoomId;

            try
            {
                SetActivity();
                AddView(CallView);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }
        }

        #endregion

        #region Helper Function(s)

        void SetActivity()
        {
            CallPageActivity = this.Context as BaseActivity;
            CallView = CallPageActivity.LayoutInflater.Inflate(Resource.Layout.activity_call, this, false);

            FullScreenRenderer = FindViewById<SurfaceViewRenderer>(Resource.Id.fullscreen_video_view);
            PipScreenRenderer = FindViewById<SurfaceViewRenderer>(Resource.Id.pip_video_view);

            CallFragment = CreateCallFragment(CallPageActivity.Intent);

            LocalVideoRenderer = new VideoRendererProxy();
            RemoteVideoRenderer = new VideoRendererProxy();

            PipScreenRenderer.Click += PipScreenRenderer_Click;
            PipScreenRenderer.SetScalingType(RendererCommon.ScalingType.ScaleAspectFit);
            PipScreenRenderer.SetZOrderMediaOverlay(true);
            PipScreenRenderer.SetEnableHardwareScaler(true);


            FullScreenRenderer.Click += FullScreenRenderer_Click;
            FullScreenRenderer.SetScalingType(RendererCommon.ScalingType.ScaleAspectFill);
            FullScreenRenderer.SetEnableHardwareScaler(false);


            SetSwappedFeeds(true);

            var fragmentTransaction = CallPageActivity.SupportFragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.call_fragment_container, CallFragment);
            fragmentTransaction.Commit();

            CallController = CreateController();
            Connect(CallController, CallPageActivity.Intent);

        }


        private void SetSwappedFeeds(bool isSwappedFeed)
        {
            IsSwappedFeed = isSwappedFeed;
            LocalVideoRenderer.Renderer = IsSwappedFeed ? FullScreenRenderer : PipScreenRenderer;
            RemoteVideoRenderer.Renderer = IsSwappedFeed ? PipScreenRenderer : FullScreenRenderer;
            FullScreenRenderer.SetMirror(isSwappedFeed);
            PipScreenRenderer.SetMirror(!isSwappedFeed);
        }

        private void ToggleCallControlFragmentVisibility()
        {
            if (!CallController.Connected || !CallFragment.IsAdded)
            {
                return;
            }

            CallControlFragmentVisible = !CallControlFragmentVisible;

            var ft = CallPageActivity.SupportFragmentManager.BeginTransaction();
            if (CallControlFragmentVisible)
            {
                ft.Show(CallFragment);
            }
            else
            {
                ft.Hide(CallFragment);
            }

            ft.SetTransition(FragmentTransaction.TransitFragmentFade);
            ft.Commit();
        }

        #region Event Handler(s)

        private void FullScreenRenderer_Click(object sender, EventArgs e) => ToggleCallControlFragmentVisibility();

        private void PipScreenRenderer_Click(object sender, EventArgs e) => SetSwappedFeeds(!IsSwappedFeed);

        #endregion

        private void Disconnect()
        {
            CallController?.Disconnect();
            CallController = null;
            PipScreenRenderer?.Release();
            PipScreenRenderer = null;
            FullScreenRenderer?.Release();
            FullScreenRenderer = null;

            CallPageActivity?.Finish();
        }

        #endregion
    }


    public interface ICallPageRenderer<TConnectionParameters, TSignalParameters, TController> : IRTCEngineEvents, CallFragment.IOnCallEvents
        where TConnectionParameters : IConnectionParameters
        where TSignalParameters : ISignalingParameters
        where TController : RTCControllerBase<TConnectionParameters, TSignalParameters>
    {
        #region Method(s)

        TController CreateController();
        void Connect(TController _controller, Intent _intent);

        CallFragment CreateCallFragment(Intent _intent);
        #endregion
    }
}
