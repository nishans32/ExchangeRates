using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateDataService
    {
        Task<ExchangeRatesDto> GetChangedRates(object rates);
        Task SaveLastUpdatedDate(object rates);
        Task SaveNewRates(object changedRates);
    }

    public class ExchangeRateDataService : IExchangeRateDataService
    {
        public Task<ExchangeRatesDto> GetChangedRates(object rates)
        {
            // Check the timestamp to see if it's newer.
            return Task.FromResult(new ExchangeRatesDto
            {
                Rates = new Dictionary<string, double>()
            });
        }

        public Task SaveLastUpdatedDate(object rates)
        {
            throw new NotImplementedException();
        }

        public Task SaveNewRates(object changedRates)
        {
            throw new NotImplementedException();
        }
    }
}
