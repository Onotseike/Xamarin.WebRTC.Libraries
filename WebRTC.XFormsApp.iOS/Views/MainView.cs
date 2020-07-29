// onotseike@hotmail.comPaula Aliu
using System;

using UIKit;

using WebRTC.XFormsApp.iOS.Views;
using WebRTC.XFormsApp.Model;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CallingView), typeof(MainView))]
namespace WebRTC.XFormsApp.iOS.Views
{
    public class MainView : ViewRenderer<CallingView, UIMainView>
    {
        UIMainView uIMainView;

        protected override void OnElementChanged(ElementChangedEventArgs<CallingView> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(uIMainView);
        }


    }
}
