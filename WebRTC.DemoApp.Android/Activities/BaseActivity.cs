// onotseike@hotmail.comPaula Aliu

using Android.OS;
using Android.Views;

using Xamarin.Forms.Platform.Android;

namespace WebRTC.DemoApp.Droid.Activities
{
    public abstract class BaseActivity : FormsAppCompatActivity
    {
        #region Properties and Variable



        #endregion


        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestWindowFeature(WindowFeatures.NoTitle);

            Window.AddFlags(WindowManagerFlags.Fullscreen | WindowManagerFlags.KeepScreenOn | WindowManagerFlags.ShowWhenLocked | WindowManagerFlags.TurnScreenOn);
            Window.DecorView.SystemUiVisibility = GetSystemUiVisibility();

            //SetContentView(Resource.Layout.activity_call);


        }



        #region HelperFunctions

        private StatusBarVisibility GetSystemUiVisibility()
        {
            var flags = SystemUiFlags.HideNavigation | SystemUiFlags.Fullscreen;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
                flags = SystemUiFlags.ImmersiveSticky;
            return (StatusBarVisibility)flags;
        }

        #endregion
    }
}
