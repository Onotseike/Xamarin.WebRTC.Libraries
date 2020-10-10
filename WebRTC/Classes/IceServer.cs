// onotseike@hotmail.comPaula Aliu
using System;
using System.Text;

using Newtonsoft.Json;

using WebRTC.Enums;

namespace WebRTC.Classes
{
    public class IceServer
    {
        public IceServer(string uri, string username = "", string password = "", TlsCertPolicy tlsCertPolicy = TlsCertPolicy.Secure) : this(new[] { uri }, username, password, tlsCertPolicy)
        {
        }

        public IceServer(string[] urls, string username, string password,
            TlsCertPolicy tlsCertPolicy = TlsCertPolicy.Secure)
        {
            Urls = urls;
            Username = username;
            Password = password;
            TlsCertPolicy = tlsCertPolicy;
        }

        public IceServer()
        {

        }
        [JsonProperty("urls")] public string[] Urls { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        [JsonProperty("password")] public string Password { get; set; }
        [JsonProperty("tlsCertPolicy")] public TlsCertPolicy TlsCertPolicy { get; }



        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var url in Urls)
            {
                sb.Append(url).Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append("] ");
            if (!string.IsNullOrEmpty(Username))
                sb.Append("[").Append(Username).Append(":").Append("] ");
            sb.Append("[").Append(TlsCertPolicy).Append("]");
            return sb.ToString();
        }
    }
}
