// onotseike@hotmail.comPaula Aliu


using Newtonsoft.Json;

using WebRTC.Classes;

namespace WebRTC.Servers.ServerFxns
{
    public class SessionDescriptionMessage : SignalingMessage
    {
        public SessionDescription Description { get; set; }

        public SessionDescriptionMessage(SessionDescription description)
        {
            Description = description;
            switch (Description.Type)
            {
                case WebRTC.Enums.SdpType.Offer:
                    SignalingMessageType = Core.Enum.SignalingMessageType.Offer;
                    break;
                case WebRTC.Enums.SdpType.Answer:
                    SignalingMessageType = Core.Enum.SignalingMessageType.Answer;
                    break;
                case WebRTC.Enums.SdpType.PrAnswer:
                    SignalingMessageType = Core.Enum.SignalingMessageType.PrAnswer;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine($"SessionDescriptionMessage unexpected type: {SignalingMessageType}");
                    break;
            }
        }

        public override string JsonData => JsonConvert.SerializeObject(new
        {
            type = GetTypeString(SignalingMessageType),
            sdp = Description.Sdp
        });
    }
}
