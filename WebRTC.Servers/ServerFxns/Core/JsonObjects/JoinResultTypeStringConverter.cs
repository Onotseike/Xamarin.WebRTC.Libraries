// onotseike@hotmail.comPaula Aliu

using System;

using Newtonsoft.Json;

using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core.JsonObjects
{
    public class JoinResultTypeStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string);


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return JoinResponseType.Unknown;

            var value = serializer.Deserialize<string>(reader);
            if (value == "SUCCESS") return JoinResponseType.Success;
            if (value == "FULL") return JoinResponseType.Full;

            return JoinResponseType.Unknown;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
    }
}