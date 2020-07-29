// onotseike@hotmail.comPaula Aliu

using System;

using WebRTC.XFormsApp.Interfaces;
using WebRTC.XFormsApp.iOS.Controllers;

using Xamarin.Forms;

[assembly: Dependency(typeof(WebRTC.XFormsApp.iOS.Implementation.Calling))]
namespace WebRTC.XFormsApp.iOS.Implementation
{
    public class Calling : ICalling
    {


        public void StartVideoCall(string roomId)
        {
            var callController = new CallController(roomId);
            //callController.CommenceCall();

        }

        public void StartVoiceCall(string roomId)
        {
            throw new NotImplementedException();
        }
    }
}
