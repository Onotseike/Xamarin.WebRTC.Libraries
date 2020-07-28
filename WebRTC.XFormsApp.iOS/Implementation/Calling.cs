// onotseike@hotmail.comPaula Aliu

using System;

using WebRTC.XFormsApp.Interfaces;

using Xamarin.Forms;

[assembly: Dependency(typeof(WebRTC.XFormsApp.iOS.Implementation.Calling))]
namespace WebRTC.XFormsApp.iOS.Implementation
{
    public class Calling : ICalling
    {


        public void StartVideoCall(string roomId)
        {
            throw new NotImplementedException();
        }

        public void StartVoiceCall(string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
