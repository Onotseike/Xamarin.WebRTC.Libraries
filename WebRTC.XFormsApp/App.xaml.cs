using System;

using WebRTC.XFormsApp.Interfaces;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WebRTC.XFormsApp
{
    public partial class App : Application
    {

        public static ICalling CallingService => DependencyService.Get<ICalling>();

        public App()
        {

            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
