using System;
using System.Threading.Tasks;

public interface IExchangeRateProcessingService
{
    Task Process();
}

public class ExchangeRateProcessingService : IExchangeRateProcessingService
{
    private readonly IExchangeRateCacheService _cacheService;
    private readonly IEventNotificationService _notificationService;
    private readonly IExchangeRateServiceClient _serviceClient;
    private readonly IExchangeRateDataService _dataService ;


    public ExchangeRateProcessingService(IExchangeRateDataService dataService, IExchangeRateCacheService cacheService, IEventNotificationService notificationService, IExchangeRateServiceClient serviceClient)
    {
        dataService = dataService;
        _cacheService = cacheService;
        _notificationService = notificationService;
        _serviceClient = serviceClient;
    }

    public async Task Process()
    {
        var rates = _serviceClient.GetRates();
        var changedRates = _dataService.GetChangedRates(rates);

        if(changedRates == null)
        {
            _dataService.SaveLastUpdatedTime(rates);
            return;
        }

        var tasks = new List<Task>
        {
            _dataService.SaveNewRates(changedRates),
            _cacheService.AddToCache(changedRates),
            _notificationService.Notify(changedRates),
        };

        await Task.WhenAll(tasks);
    }
}

