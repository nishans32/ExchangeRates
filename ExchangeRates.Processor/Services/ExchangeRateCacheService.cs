using System.Threading.Tasks;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateCacheService
    {
        Task AddToCache(object changedRates);
    }
    public class ExchangeRateCacheService : IExchangeRateCacheService
    {
        public Task AddToCache(object changedRates)
        {
            throw new System.NotImplementedException();
        }
    }
}