// onotseike@hotmail.comPaula Aliu

using Newtonsoft.Json;

using WebRTC.Classes;

namespace WebRTC.Servers.ServerFxns
{
    public class ICECandidateRemovalMessage : SignalingMessage
    {
        public IceCandidate[] IceCandidates { get; set; }

        public ICECandidateRemovalMessage(IceCandidate[] iceCandidates)
        {
            SignalingMessageType = Core.Enum.SignalingMessageType.CandidateRemoval;
            IceCandidates = iceCandidates;
        }

        public override string JsonData => JsonConvert.SerializeObject(new
        {
            type = GetTypeString(SignalingMessageType),
            candidates = ToJsonCandidates(IceCandidates)
        });
    }
}
