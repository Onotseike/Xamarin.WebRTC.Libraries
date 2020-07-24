// onotseike@hotmail.comPaula Aliu
using System;

using Newtonsoft.Json;

using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core.JsonObjects
{
    public class MessageResponse
    {
        private string _result;

        [JsonProperty("result")]
        public string Result
        {
            get => _result;
            set { _result = value; Type = MessageResultTypeFromString(_result); }
        }

        public MessageResultType Type { get; set; }

        public static MessageResultType MessageResultTypeFromString(string resultString)
        {
            MessageResultType result = MessageResultType.Unknown;
            if (resultString.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase))
            {
                result = MessageResultType.Success;
            }
            else if (resultString.Equals("INVALID_CLIENT", StringComparison.OrdinalIgnoreCase))
            {
                result = MessageResultType.InvalidClient;
            }
            else if (resultString.Equals("INVALID_ROOM", StringComparison.OrdinalIgnoreCase))
            {
                result = MessageResultType.InvalidRoom;
            }
            return result;
        }

    }
}
