// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;

namespace WebRTC.Signal.Server.Models
{
    public class Client
    {
        #region Properties

        public Guid ClientId { get; set; }
        public bool InRoom { get; set; }
        public bool IsInitiator { get; set; }
        public List<SignalMessage> Messages { get; set; }

        #endregion

        public Client(string _clientId)
        {
            ClientId = new Guid(_clientId);
            Messages = new List<SignalMessage>();
        }


        #region Function(s)

        public void AddMessage(SignalMessage _signalMessage) => Messages?.Add(_signalMessage);
        public void AddMessages(List<SignalMessage> _signalMessages) => Messages?.AddRange(_signalMessages);

        public void ClearMessage(SignalMessage _signalMessage) => Messages?.RemoveAll(message => message.MesssageId == _signalMessage.MesssageId);
        public void ClearAllMessages() => Messages?.Clear();

        #endregion
    }
}
