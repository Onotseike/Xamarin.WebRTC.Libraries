// onotseike@hotmail.comPaula Aliu
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using WebRTC.Servers.ServerFxns.Core.Enum;

namespace WebRTC.Servers.ServerFxns.Core
{
    public delegate void AsyncHttpCallback(string response, string errorMessage);

    public class AsyncHttpURLConnection
    {
        private const string HttpOrigin = "https://appr.tc";

        private static readonly HttpClient HttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(8) };

        private readonly HttpMethodType httpMethodType;
        private readonly Uri url;
        private readonly string message;
        private readonly AsyncHttpCallback asyncHttpCallback;

        private string contentType;


        static AsyncHttpURLConnection() => HttpClient.DefaultRequestHeaders.Add("origin", HttpOrigin);

        public AsyncHttpURLConnection(HttpMethodType _methodType, string _url, string _message, AsyncHttpCallback _callback)
        {
            httpMethodType = _methodType;
            url = new Uri(_url);
            message = _message;
            asyncHttpCallback = _callback;
        }

        public void SetContentType(string _contentType) => contentType = _contentType;

        public void Send() => Task.Factory.StartNew(SendHttpMessageAsync);


        #region Helper Functions(Private & Protected)

        private async Task SendHttpMessageAsync()
        {
            contentType = contentType ?? "text/plain";
            HttpResponseMessage httpResponseMessage;

            try
            {
                switch (httpMethodType)
                {
                    case HttpMethodType.Post:
                        var content = new StringContent(message, Encoding.UTF8, contentType);
                        httpResponseMessage = await HttpClient.PostAsync(url, content);
                        break;
                    case HttpMethodType.Get:
                        httpResponseMessage = await HttpClient.GetAsync(url);
                        break;
                    case HttpMethodType.Delete:
                        httpResponseMessage = await HttpClient.DeleteAsync(url);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();

                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    asyncHttpCallback?.Invoke(null, $"Non-200 Response to {httpMethodType} to URL {url}");
                    return;
                }

                if (httpMethodType == HttpMethodType.Delete)
                {
                    asyncHttpCallback?.Invoke(" ", null);
                    return;
                }

                var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                asyncHttpCallback?.Invoke(responseContent, null);
            }
            catch (Exception ex)
            {
                asyncHttpCallback?.Invoke(null, $"HTTP {httpMethodType} to {url} error: {ex.Message}");
            }
        }

        #endregion

    }
}
