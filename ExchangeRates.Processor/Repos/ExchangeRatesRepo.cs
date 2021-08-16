using ExchangeRates.Processor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRates.Processor.Repos
{
    public interface IExchangeRatesRepo
    {
        Task<IEnumerable<ExchangeRate>> GetRates();
    }

    public class ExchangeRatesRepo : IExchangeRatesRepo
    {
        public async Task<IEnumerable<ExchangeRate>> GetRates()
        {
            return await Task.FromResult(new List<ExchangeRate>() { new ExchangeRate {  Code = "AED", Value = 145.50M } });
        }
    }
}