using ExchangeRates.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using ExchangeRates.Processor.Models;

namespace ExchangeRates.Processor.Mappers
{
    public interface IExchangeRatesMapper
    {
        ExchangeRateBatch MapToExchangeRateBatch(ExchangeRatesDto newRates, Dictionary<string, string> rateNames);
        IEnumerable<ExchangeRateEvent> MapToExchangeRateEvent(ExchangeRatesDto newRates);
        ChangedRates MapExchangeRateNames(ChangedRates rates, Dictionary<string, string> names);
        ChangedRates MapChangedRates(IEnumerable<ExchangeRate> addedRates, IEnumerable<ExchangeRate> updatedRates, IEnumerable<ExchangeRate> deletedRates);
    }

    /// <summary>
    /// Maybe use automapper later on 
    /// </summary>
    public class ExchangeRatesMapper : IExchangeRatesMapper
    {
        public ExchangeRateBatch MapToExchangeRateBatch(ExchangeRatesDto newRates, Dictionary<string, string> rateNames)
        {
            return new ExchangeRateBatch
            {
                BatchId = new Guid(),
                Rates = newRates.Rates.Select(rate => new ExchangeRate
                {
                    Code = rate.Key,
                    Value = Decimal.Round(rate.Value, 4),
                    LastUpdatedUtc = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    Name = rateNames[rate.Key]
                })
            };
        }

        public IEnumerable<ExchangeRateEvent> MapToExchangeRateEvent(ExchangeRatesDto newRates)
        {
            var batchId = Guid.NewGuid();
            return newRates.Rates.Select(rate => new ExchangeRateEvent
            {
                BatchId = batchId,
                Code = rate.Key,
                Value = Decimal.Round(rate.Value, 4),
                LastUpdatedUtc = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
        }

        public ChangedRates MapExchangeRateNames(ChangedRates rates, Dictionary<string, string> names)
        {
            rates
                .AddedRates.ToList()
                .ForEach(rate => rate.Name = names[rate.Code]);

            rates
                .AddedRates.ToList()
                .ForEach(rate => rate.Name = names[rate.Code]);

            return rates;
        }

        public ChangedRates MapChangedRates(IEnumerable<ExchangeRate> addedRates, IEnumerable<ExchangeRate> updatedRates, IEnumerable<ExchangeRate> deletedRates)
        {
            return new ChangedRates
            {
                AddedRates = addedRates,
                UpdatedRates = updatedRates,
                DeletedRates = deletedRates
            };
        }
    }
}