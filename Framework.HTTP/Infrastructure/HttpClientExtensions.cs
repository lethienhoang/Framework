using Framework.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Framework.HTTP.Infrastructure
{
    public class HttpClientExtensions : IHttpClient
    {
        private const string ApplicationJsonContentType = "application/json";

        private static readonly StringContent EmptyJson =
            new StringContent("{}", Encoding.UTF8, ApplicationJsonContentType);

        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientExtensions(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await SendAsync(httpClient, request);
        }

        public async Task<T> DeleteAsync<T>(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await SendAsync<T>(httpClient, request);
        }

        public async Task<HttpResult<T>> DeleteResultAsync<T>(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await SendResultAsync<T>(httpClient, request);
        }

        public async Task<HttpResponseMessage> GetAsync(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            return await SendAsync(httpClient, request);
        }

        public async Task<T> GetAsync<T>(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            return await SendAsync<T>(httpClient, request);
        }

        public async Task<HttpResult<T>> GetResultAsync<T>(string clientName, string uri)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            return await SendResultAsync<T>(httpClient, request);
        }

        public async Task<HttpResponseMessage> PostAsync(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendAsync(httpClient, request);
        }

        public async Task<T> PostAsync<T>(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendAsync<T>(httpClient, request);
        }

        public async Task<HttpResult<T>> PostResultAsync<T>(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendResultAsync<T>(httpClient, request);
        }

        public async Task<HttpResponseMessage> PutAsync(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendAsync(httpClient, request);
        }

        public async Task<T> PutAsync<T>(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendAsync<T>(httpClient, request);
        }

        public async Task<HttpResult<T>> PutResultAsync<T>(string clientName, string uri, object data = null)
        {
            var httpClient = _httpClientFactory.CreateClient(clientName);
            var request = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = ToJsonString(data)
            };

            return await SendResultAsync<T>(httpClient, request);
        }

        #region Setting up

        private async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, HttpRequestMessage request)
        {
            return await httpClient.SendAsync(request);
        }

        private async Task<T> SendAsync<T>(HttpClient httpClient, HttpRequestMessage request)
        {
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var stream = await response.Content.ReadAsStreamAsync();

            return JsonConverterHelpers.DeserializeJsonFromStream<T>(stream);
        }

        private async Task<HttpResult<T>> SendResultAsync<T>(HttpClient httpClient, HttpRequestMessage request)
        {
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var stream = await response.Content.ReadAsStreamAsync();
            var result = JsonConverterHelpers.DeserializeJsonFromStream<T>(stream);

            return new HttpResult<T>(result, response);
        }

        public static StringContent ToJsonString(object data)
        {
            if (data == null)
            {
                return EmptyJson;
            }

            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, ApplicationJsonContentType);
        }
        #endregion
    }
}
