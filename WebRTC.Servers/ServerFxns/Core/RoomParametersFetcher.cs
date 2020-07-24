// onotseike@hotmail.comPaula Aliu

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using WebRTC.Classes;
using WebRTC.Servers.Interfaces;
using WebRTC.Servers.Models;
using WebRTC.Servers.ServerFxns.Core.Enum;
using WebRTC.Servers.ServerFxns.Core.JsonObjects;

namespace WebRTC.Servers.ServerFxns.Core
{
    public class RoomParametersFetcher
    {
        private const string TAG = nameof(RoomParametersFetcher);
        private readonly string roomURL;
        private readonly string roomMessage;

        private readonly ILogger logger;

        public RoomParametersFetcher(string _roomURL, string _roomMessage, ILogger _logger = null)
        {
            roomURL = _roomURL;
            roomMessage = _roomMessage;
            logger = _logger ?? new ConsoleLogger();
        }

        public delegate void RoomParametersFetcherCallback(SignalingParameters signalingParameters, string errorDescription);


        public void MakeRequest(RoomParametersFetcherCallback callback)
        {
            var httpConnection = new AsyncHttpURLConnection(HttpMethodType.Post, roomURL, null, async (response, errorMessage) =>
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


        #region Helper Functions (Private)

        private async Task RoomResponseParseAsync(string response, RoomParametersFetcherCallback callback)
        {
            logger.Debug(TAG, $"Room response: {response}");
            try
            {
                var joinResponse = JsonConvert.DeserializeObject<JoinResponseHelper>(response);

                if (joinResponse.Result != JoinResponseType.Success)
                {
                    callback?.Invoke(null, $"Room response error: {joinResponse.Result}");
                    return;
                }


                var iceCandidates = new List<IceCandidate>();

                SessionDescription offerSdp = null;

                var serverParams = joinResponse.ServerParameters;

                if (!serverParams.IsInitiator)
                {
                    var i = 0;
                    foreach (var messageString in serverParams.Messages)
                    {
                        logger.Debug(TAG, $"GAE-> # {i} {messageString}");
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
                                logger.Error(TAG, $"Unknown message: {messageString}");
                                break;
                        }

                        i++;
                    }
                }

                logger.Debug(TAG, $"RoomId: {serverParams.RoomId}. ClientId: {serverParams.ClientId}");
                logger.Debug(TAG, $"Initiator: {serverParams.IsInitiator}");
                logger.Debug(TAG, $"WSS url: {serverParams.WssUrl}");
                logger.Debug(TAG, $"WSS POST url: {serverParams.WssPostUrl}");


                var iceServers = new List<Classes.IceServer>();

                iceServers.AddRange(IceServersFromPCConfig(serverParams.PcConfig));

                var isTurnPresent = false;
                foreach (var iceServer in iceServers)
                {
                    logger.Debug(TAG, $"IceServer: {iceServer}");
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
                        logger.Debug(TAG, $"TURN Server: {iceServer}");
                    }

                    iceServers.AddRange(turnsServers.iceServers);
                }

                var signalingParameters = new SignalingParameters()
                {
                    IceServers = iceServers.ToArray(),
                    IsInitiator = serverParams.IsInitiator,
                    ClientId = serverParams.ClientId,
                    WssURL = serverParams.WssUrl,
                    WssPostURL = serverParams.WssPostUrl,
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

        private async Task<(Classes.IceServer[] iceServers, string error)> RequestTurnServersAsync(string iceServerUrl)
        {
            logger.Debug(TAG, $"REQUEST TURN from: {iceServerUrl}");

            var turnClient = new TURNClient(iceServerUrl);

            try
            {
                var turnServers = await turnClient.RequestServersAsync();
                return (turnServers, null);
            }
            catch (Exception)
            {
                return (null, $"NON-200 Response when requesting TURN Server from: {iceServerUrl}");
            }
        }

        private Classes.IceServer[] IceServersFromPCConfig(string json)
        {
            var pcConfig = JsonConvert.DeserializeObject<PCConfig>(json);
            var iceServers = new List<Classes.IceServer>();

            foreach (var iceServer in pcConfig.IceServers)
            {
                var server = JsonConvert.DeserializeObject<Dictionary<string, string>>(iceServer);

                var url = server["urls"];
                var credentials = server.ContainsKey("credential") ? server["credential"] : "";

                iceServers.Add(new Classes.IceServer(uri: url, username: "", password: credentials));
            }

            return iceServers.ToArray();
        }

        #endregion
    }
}
