// onotseike@hotmail.comPaula Aliu
#region Using Statements
using System;

using WebRTC.XFormsApp.Interfaces;

using Xamarin.Forms;

#endregion


[assembly: Dependency(typeof(WebRTC.XFormsApp.Droid.Implementation.Calling))]
namespace WebRTC.XFormsApp.Droid.Implementation
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
