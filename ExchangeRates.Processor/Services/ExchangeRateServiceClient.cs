using ExchangeRates.Processor.Lib;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateServiceClient
    {
        Task<ExchangeRatesDto> GetRates();
    }

    public class ExchangeRateServiceClient : IExchangeRateServiceClient
    {
        private readonly IHttpClientService _httpClient;
        private readonly IOptions<ExchangeRatesApiSettings> _httpOptions;

        public ExchangeRateServiceClient(IHttpClientService httpClient, IOptions<ExchangeRatesApiSettings> httpOptions)
        {
            _httpClient = httpClient;
            _httpOptions = httpOptions;
        }

        public async Task<ExchangeRatesDto> GetRates()
        {
            return await _httpClient.Get<ExchangeRatesDto>(_httpOptions.Value.Url);
        }
    }
}