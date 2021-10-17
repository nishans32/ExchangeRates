using ExchangeRates.Importer.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ExchangeRates.Processor.Mappers
{
    public interface IExchangeRatesMapper
    {
        IEnumerable<ExchangeRate> MapToExchangeRate(ExchangeRatesDto newRates);
        IEnumerable<ExchangeRateEvent> MapToExchangeRateEvent(ExchangeRatesDto newRates);

    }

    /// <summary>
    /// Maybe use automapper later on 
    /// </summary>
    public class ExchangeRatesMapper : IExchangeRatesMapper
    {
        public IEnumerable<ExchangeRate> MapToExchangeRate(ExchangeRatesDto newRates)
        {
            var batchId = Guid.NewGuid();
            return newRates.Rates.Select(rate => new ExchangeRate 
            { 
                Code = rate.Key, 
                Value = rate.Value, 
                LastUpdatedUtc = DateTime.UtcNow, 
                Id = Guid.NewGuid()
            });
        }

        public IEnumerable<ExchangeRateEvent> MapToExchangeRateEvent(ExchangeRatesDto newRates)
        {
            var batchId = Guid.NewGuid();
            return newRates.Rates.Select(rate => new ExchangeRateEvent
            {
                BatchId = batchId,
                Code = rate.Key,
                Value = rate.Value,
                LastUpdatedUtc = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
        }
    }
}