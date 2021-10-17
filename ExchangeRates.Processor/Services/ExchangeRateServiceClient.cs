using ExchangeRates.Processor.Lib;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateServiceClient
    {
        Task<ExchangeRatesDto> GetRates();
        Task<Dictionary<string, string>> GetRatesNames();
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
            return await _httpClient.Get<ExchangeRatesDto>(_httpOptions.Value.RatesUrl);
        }

        public async Task<Dictionary<string, string>> GetRatesNames()
        {
            return await _httpClient.Get<Dictionary<string, string>>(_httpOptions.Value.RatesNamesUrl);
        }
    }
}