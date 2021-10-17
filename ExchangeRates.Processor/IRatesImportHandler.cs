using ExchangeRates.Common.Models;
using ExchangeRates.Processor.Mappers;
using ExchangeRates.Processor.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRatesImporter
{
    Task<ExchangeRateBatch> Import();
}

public class RatesImporter : IRatesImporter
{
    private readonly IExchangeRateServiceClient _exchangeRatesServiceClient;
    private readonly IExchangeRatesMapper _mapper;

    public RatesImporter(IExchangeRateServiceClient exchangeRateServiceClient, IExchangeRatesMapper mapper)
    {
        _exchangeRatesServiceClient = exchangeRateServiceClient;
        _mapper = mapper;
    }
    public async Task<ExchangeRateBatch> Import()
    {
        var ratesTask = _exchangeRatesServiceClient.GetRates();
        var ratesNamesTask = _exchangeRatesServiceClient.GetRatesNames();
        await Task.WhenAll(ratesTask, ratesNamesTask);

        return _mapper.MapToExchangeRateBatch(ratesTask.Result, ratesNamesTask.Result);
    }
}
