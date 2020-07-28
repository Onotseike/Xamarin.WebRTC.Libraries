// onotseike@hotmail.comPaula Aliu
using System;
namespace WebRTC.XFormsApp.Interfaces
{
    public interface ICalling
    {
        void StartVideoCall(string roomId);
        void StartVoiceCall(string roomId);
    }
}
