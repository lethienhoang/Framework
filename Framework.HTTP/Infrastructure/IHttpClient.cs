using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.HTTP.Infrastructure
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string clientName, string uri);
        Task<T> GetAsync<T>(string clientName, string uri);
        Task<HttpResult<T>> GetResultAsync<T>(string clientName, string uri);
        Task<HttpResponseMessage> PostAsync(string clientName, string uri, object data = null);
        Task<T> PostAsync<T>(string clientName, string uri, object data = null);
        Task<HttpResult<T>> PostResultAsync<T>(string clientName, string uri, object data = null);
        Task<HttpResponseMessage> PutAsync(string clientName, string uri, object data = null);
        Task<T> PutAsync<T>(string clientName, string uri, object data = null);
        Task<HttpResult<T>> PutResultAsync<T>(string clientName, string uri, object data = null);
        Task<HttpResponseMessage> DeleteAsync(string clientName, string uri);
        Task<T> DeleteAsync<T>(string clientName, string uri);
        Task<HttpResult<T>> DeleteResultAsync<T>(string clientName, string uri);
    }
}
