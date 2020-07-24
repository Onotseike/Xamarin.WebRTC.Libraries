// onotseike@hotmail.comPaula Aliu
using Newtonsoft.Json;

namespace WebRTC.Servers.ServerFxns
{
    public class ByeMessage : SignalingMessage
    {
        public ByeMessage()
        {
            SignalingMessageType = Core.Enum.SignalingMessageType.Bye;
        }

        public override string JsonData => JsonConvert.SerializeObject(new { type = "bye" });
    }
}
