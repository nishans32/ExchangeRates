using ExchangeRates.Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IExchangeRateImporter
{
    Task Process();
}

public class ExchangeRateImporter : IExchangeRateImporter
{
    private readonly IExchangeRateCacheService _cacheService;
    private readonly IEventNotificationService _notificationService;
    private readonly IExchangeRateServiceClient _serviceClient;
    private readonly IExchangeRateDataService _dataService ;


    public ExchangeRateImporter(IExchangeRateDataService dataService, IExchangeRateCacheService cacheService, IEventNotificationService notificationService, IExchangeRateServiceClient serviceClient)
    {
        _dataService = dataService;
        _cacheService = cacheService;
        _notificationService = notificationService;
        _serviceClient = serviceClient;
    }

    public async Task Process()
    {
        var rates = await _serviceClient.GetRates();
        var changedRates = await _dataService.GetChangedRates(rates);

        if(!changedRates.Any())
        {
            await _dataService.SaveLastUpdatedDate(rates);
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

