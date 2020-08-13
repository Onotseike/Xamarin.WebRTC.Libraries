// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using Newtonsoft.Json;

using WebRTC.Classes;
using WebRTC.RTC.Abstraction;



namespace WebRTC.RTC
{
    public class RoomParametersFetcher
    {
        private const string TAG = nameof(RoomParametersFetcher);
        private readonly string _roomUrl;
        private readonly string _roomMessage;

        private readonly ILogger _logger;




        #region Constructor(s)

        public RoomParametersFetcher(string roomUrl, string roomMessage, ILogger logger = null)
        {
            _roomUrl = roomUrl;
            _roomMessage = roomMessage;
            _logger = logger ?? new ConsoleLogger();
        }

        #endregion



        public delegate void RoomParametersFetcherCallback(SignalingParameters signalingParameters,
            string errorDescription);

        public void MakeRequest(RoomParametersFetcherCallback callback)
        {
            var httpConnection = new AsyncHttpURLConnection(MethodType.Post, _roomUrl, null, async (response, errorMessage) =>
            {
                if (errorMessage != null)
                {
                    callback?.Invoke(null, errorMessage);
                    return;
                }

                await RoomResponseParseAsync(response, callback);
            });
            httpConnection.Send();
        }

        private async Task RoomResponseParseAsync(string response, RoomParametersFetcherCallback callback)
        {
            _logger.Debug(TAG, $"Room response: {response}");
            try
            {
                var joinResponse = JsonConvert.DeserializeObject<JoinResponse>(response);

                if (joinResponse.Result != JoinResultType.Success)
                {
                    callback?.Invoke(null, $"Room response error: {joinResponse.Result}");
                    return;
                }


                var iceCandidates = new List<IceCandidate>();

                SessionDescription offerSdp = null;

                var serverParams = joinResponse.ServerParams;

                if (!serverParams.IsInitiator)
                {
                    var i = 0;
                    foreach (var messageString in serverParams.Messages)
                    {
                        _logger.Debug(TAG, $"GAE-> # {i} {messageString}");
                        var message = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageString);
                        var messageType = message["type"];
                        switch (messageType)
                        {
                            case "offer":
                                offerSdp =
                                    new SessionDescription(SessionDescription.GetSdpTypeFromString(messageType),
                                        message["sdp"]);
                                break;
                            case "candidate":

                                var iceCandidate = new IceCandidate(message["candidate"], message["id"],
                                    int.Parse(message["label"]));
                                iceCandidates.Add(iceCandidate);
                                break;
                            default:
                                _logger.Error(TAG, $"Unknown message: {messageString}");
                                break;
                        }

                        i++;
                    }
                }

                _logger.Debug(TAG, $"RoomId: {serverParams.RoomId}. ClientId: {serverParams.ClientId}");
                _logger.Debug(TAG, $"Initiator: {serverParams.IsInitiator}");
                _logger.Debug(TAG, $"WSS url: {serverParams.WssUrl}");
                _logger.Debug(TAG, $"WSS POST url: {serverParams.WssPostUrl}");


                var iceServers = new List<IceServer>();

                iceServers.AddRange(IceServersFromPCConfig(serverParams.PcConfig));

                var isTurnPresent = false;
                foreach (var iceServer in iceServers)
                {
                    _logger.Debug(TAG, $"IceServer: {iceServer}");
                    foreach (var url in iceServer.Urls)
                    {
                        if (url.StartsWith("turn:"))
                        {
                            isTurnPresent = true;
                            break;
                        }
                    }
                }


                if (!isTurnPresent && !string.IsNullOrEmpty(serverParams.IceServerUrl))
                {
                    var turnsServers = await RequestTurnServersAsync(serverParams.IceServerUrl);
                    if (turnsServers.error != null)
                    {
                        callback?.Invoke(null, turnsServers.error);
                        return;
                    }

                    foreach (var iceServer in turnsServers.iceServers)
                    {
                        _logger.Debug(TAG, $"TurnServer: {iceServer}");
                    }

                    iceServers.AddRange(turnsServers.iceServers);
                }

                var signalingParameters = new SignalingParameters()
                {
                    IceServers = iceServers.ToArray(),
                    IsInitiator = serverParams.IsInitiator,
                    ClientId = serverParams.ClientId,
                    WssUrl = serverParams.WssUrl,
                    WssPostUrl = serverParams.WssPostUrl,
                    OfferSdp = offerSdp,
                    IceCandidates = iceCandidates.ToArray()
                };

                callback?.Invoke(signalingParameters, null);
            }
            catch (JsonException ex)
            {
                callback?.Invoke(null, $"Room JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                callback?.Invoke(null, $"Room error: {ex.Message}");
            }
        }

        private async Task<(IceServer[] iceServers, string error)> RequestTurnServersAsync(string url)
        {
            _logger.Debug(TAG, $"Request TURN from: {url}");

            var turnClient = new TURNClient(url);

            try
            {
                var turnServers = await turnClient.RequestServersAsync();
                return (turnServers, null);
            }
            catch (Exception)
            {
                return (null, $"Non-200 response when requesting TURN server from: {url}");
            }
        }

        private static IceServer[] IceServersFromPCConfig(string json)
        {
            var pcConfig = JsonConvert.DeserializeObject<PCConfig>(json);

            var iceServers = new List<IceServer>();


            foreach (var iceServer in pcConfig.IceServers)
            {
                var server = JsonConvert.DeserializeObject<Dictionary<string, string>>(iceServer);

                var url = server["urls"];
                var credential = server.ContainsKey("credential") ? server["credential"] : "";

                iceServers.Add(new IceServer(url, "", credential));
            }

            return iceServers.ToArray();
        }


        #region SignalR Hub Connection Methods

        #endregion
    }
}
