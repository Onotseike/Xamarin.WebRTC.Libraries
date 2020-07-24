// onotseike@hotmail.comPaula Aliu

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using WebRTC.Classes;
using WebRTC.Servers.ServerFxns.Core.JsonObjects;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class TURNClient
    {
        private const string TURNRefererURLString = @"https://appr.tc";
        private readonly HttpClient httpClient = new HttpClient();


        private readonly Uri iceServerUrl;

        public TURNClient(string _iceServerUrl)
        {
            this.iceServerUrl = new Uri(_iceServerUrl);
            httpClient.DefaultRequestHeaders.Referrer = new Uri(TURNRefererURLString);
        }

        public async Task<Classes.IceServer[]> RequestServersAsync()
        {
            var response = await httpClient.PostAsync(iceServerUrl, new StringContent("")).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var turnResponse = JsonConvert.DeserializeObject<TurnResponse>(json);

            var array = turnResponse.IceServers.Select(iceServer => new Classes.IceServer(iceServer.Username, iceServer.Credential)).ToArray();

            return array;
        }
    }
}