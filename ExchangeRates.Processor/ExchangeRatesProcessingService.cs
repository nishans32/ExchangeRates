using System;
using System.Threading.Tasks;

public interface IExchangeRateProcessingService
{
    Task Process();     
}

public class ExchangeRateProcessingService : IExchangeRateProcessingService
{
    public ExchangeRateProcessingService(IEx)
    {
    }

    public async Task Process()
    {
        var rates = _exchangeRateClientService.GetRates();
        var tasks = new List<Task>
        {
            _repo.SaveRateEvents(rates),
            _cacheService.AddToCache(rates),
            _notificationService.Notify(rates),
        };

        await Task.WhenAll(tasks);
    }
}

