using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRates.Processor.Lib
{
    public interface IHttpClientService
    {
        Task<T> Get<T>(string url);
    }
    /// <summary>
    /// Wraps httpClient, which allows clients to use a solid contract
    /// </summary>
    public class HttpClientService: IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public HttpClientService(IHttpClientFactory httpClientFactory, IJsonSerializer jsonSerializer )
        {
            _httpClient = httpClientFactory.CreateClient();
            _jsonSerializer = jsonSerializer;
        }

        public async Task<T> Get<T>(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return _jsonSerializer.Deserialize<T>(response);
        }
    }


}