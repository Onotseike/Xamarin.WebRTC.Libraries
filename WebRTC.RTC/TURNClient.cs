// onotseike@hotmail.comPaula Aliu
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using WebRTC.Classes;

namespace WebRTC.RTC
{
    public class TURNClient
    {
        private const string TURNRefererURLString = @"https://appr.tc";

        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Uri _url;

        public TURNClient(string url)
        {
            _url = new Uri(url);

            _httpClient.DefaultRequestHeaders.Referrer = new Uri(TURNRefererURLString);
        }

        public async Task<IceServer[]> RequestServersAsync()
        {
            var response = await _httpClient.PostAsync(_url, new StringContent("")).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var turnResponse = JsonConvert.DeserializeObject<TurnResponse>(json);

            var array = turnResponse.IceServers
                .Select(iceServer => new IceServer(iceServer.Urls, iceServer.Username, iceServer.Credential)).ToArray();

            return array;
        }

        public async Task<IceServer[]> GetXirSysIceServersAsync(string urlPath, string authorization)
        {
            using (var httpsClient = new HttpClient())
            {
                httpsClient.BaseAddress = new Uri(urlPath);
                httpsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", EncodingAuthString(authorization));
                httpsClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpsClient.PutAsync("", new StringContent("")).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jsonObject = JObject.Parse(responseContent);

                jsonObject.TryGetValue("v", out JToken iceServersJson);
                if (iceServersJson != null)
                {
                    var serversObject = iceServersJson["iceServers"];

                    if (serversObject != null)
                    {
                        var xirSysServers = serversObject.ToObject<XirSysIceServer[]>();

                        var groupedServers = xirSysServers.GroupBy(
                            server => server.Username,
                            server => (server.Credential, server.Url),
                            (username, cred_url) =>
                            new IceServer(urls: cred_url.Select(cu => cu.Url).ToArray(), username: username, password: cred_url.Select(cu => cu.Credential).Distinct().FirstOrDefault())
                        ).ToArray();

                        return groupedServers;
                    }
                    return null;
                }
                return null;
            }

        }

        public string EncodingAuthString(string toEncode)
        {
            byte[] bytes = Encoding.GetEncoding(28591).GetBytes(toEncode);
            string toReturn = Convert.ToBase64String(bytes);
            return toReturn;
        }
    }
}
