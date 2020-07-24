// onotseike@hotmail.comPaula Aliu

using Newtonsoft.Json;

using WebRTC.Classes;

namespace WebRTC.Servers.ServerFxns
{
    public class ICECandidateMessage : SignalingMessage
    {
        public IceCandidate Candidate { get; set; }

        public ICECandidateMessage(IceCandidate _candidate)
        {
            SignalingMessageType = Core.Enum.SignalingMessageType.Candidate;
            Candidate = _candidate;
        }

        public override string JsonData => JsonConvert.SerializeObject(new
        {
            type = GetTypeString(SignalingMessageType),
            label = Candidate.SdpMLineIndex,
            id = Candidate.SdpMid,
            candidate = Candidate.Sdp
        });
    }
}
