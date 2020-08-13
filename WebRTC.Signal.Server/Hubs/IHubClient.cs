// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using WebRTC.Signal.Server.Models;

namespace WebRTC.Signal.Server.Hubs
{
    public interface IHubClient
    {
        void IncomingCall(string callerId);

        void CallDeclined(string _clientId, string _message, string _username);
        void CallDeclined(string _clientId, string _message);

        void CallEnded(string _targetClientId, string _message, string _username);
        void CallEnded(string _targetClientId, string _message);

        void CallAccepted(Client caller);

        void ReceiveSignal(Client caller, string signal);

        void UpdateHubClientsList(JArray _clients);

    }
}
