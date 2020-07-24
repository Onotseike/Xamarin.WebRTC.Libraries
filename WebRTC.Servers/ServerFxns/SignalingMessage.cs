// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using WebRTC.Classes;
using WebRTC.Enums;
using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns
{
    public class SignalingMessage
    {
        protected const string CandidateType = "candidate";
        protected const string CandidateRemovalType = "remove-candidates";
        protected const string OfferType = "offer";
        protected const string AnswerType = "answer";
        protected const string PrAnswerType = "pranswer";
        protected const string ByeType = "bye";

        public SignalingMessageType SignalingMessageType { get; set; }

        public static string CreateJson(IceCandidate iceCandidate) => new ICECandidateMessage(iceCandidate).JsonData;
        public static string CreateJson(SessionDescription sessionDescription) => new SessionDescriptionMessage(sessionDescription).JsonData;
        public static string CreateJson(IceCandidate[] iceCandidates) => new ICECandidateRemovalMessage(iceCandidates).JsonData;

        public static string CreateByeJson() => new ByeMessage().JsonData;

        public virtual string JsonData { get; } = "{}";

        public override string ToString() => JsonData;

        public string ToStringPrettified()
        {
            var obj = new { type = SignalingMessageType.ToString(), message = JsonData.ToString() };
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static SignalingMessage MessageFromJSONString(string json)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            SignalingMessage message = new SignalingMessage();

            if (values.ContainsKey("type"))
            {
                var type = values["type"] ?? "";

                switch (type)
                {
                    case CandidateType:
                        int.TryParse(values["label"], out int label);
                        var candidate = new IceCandidate(values["candidate"], values["id"], label);
                        message = new ICECandidateMessage(candidate);
                        break;
                    case CandidateRemovalType:

                        break;
                    case OfferType:
                        var description = new SessionDescription(SdpType.Offer, values["sdp"]);
                        message = new SessionDescriptionMessage(description);
                        break;
                    case AnswerType:
                        description = new SessionDescription(SdpType.Answer, values["sdp"]);
                        message = new SessionDescriptionMessage(description);
                        break;
                    case PrAnswerType:
                        description = new SessionDescription(SdpType.PrAnswer, values["sdp"]);
                        message = new SessionDescriptionMessage(description);
                        break;
                    case ByeType:
                        message = new ByeMessage();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine($"ARDSignalingMessage unexpected type: {type}");
                        break;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"SignalingMessage invalid json: {json}");
            }

            return message;
        }

        protected static string ToJsonCandidate(IceCandidate iceCandidate)
        {
            return JsonConvert.SerializeObject(new
            {
                label = iceCandidate.SdpMLineIndex,
                id = iceCandidate.SdpMid,
                candidate = iceCandidate.Sdp
            });
        }

        protected static string ToJsonCandidates(IEnumerable<IceCandidate> iceCandidates) => JsonConvert.SerializeObject(iceCandidates.Select(ToJsonCandidate));

        protected static string GetTypeString(SignalingMessageType type)
        {
            switch (type)
            {
                case SignalingMessageType.Candidate:
                    return CandidateType;
                case SignalingMessageType.CandidateRemoval:
                    return CandidateRemovalType;
                case SignalingMessageType.Offer:
                    return OfferType;
                case SignalingMessageType.PrAnswer:
                case SignalingMessageType.Answer:
                    return AnswerType;
                //return PrAnswerType;
                case SignalingMessageType.Bye:
                    return ByeType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
