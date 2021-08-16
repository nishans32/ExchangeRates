using ExchangeRates.Processor.Models;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRates.Processor.Mappers
{
    public interface IExchangeRatesMapper
    {
        IEnumerable<ExchangeRate> MapRatesToModel(ExchangeRatesDto newRates);
    }

    /// <summary>
    /// Maybe use automapper later on 
    /// </summary>
    public class ExchangeRatesMapper : IExchangeRatesMapper
    {
        public IEnumerable<ExchangeRate> MapRatesToModel(ExchangeRatesDto newRates)
        {
            return newRates.Rates.Select(rate => new ExchangeRate { Code = rate.Key, Value = rate.Value });
        }
    }
}