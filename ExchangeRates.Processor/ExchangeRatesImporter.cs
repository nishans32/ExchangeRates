using ExchangeRates.Importer.Services;
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
    private readonly IExchangeRateServiceClient _exchangeRateserviceClient;
    private readonly IExchangeRateDataService _dataService ;

    public ExchangeRateImporter(IExchangeRateDataService dataService, IExchangeRateCacheService cacheService, IEventNotificationService notificationService, IExchangeRateServiceClient exchangeRateserviceClient)
    {
        _dataService = dataService;
        _cacheService = cacheService;
        _notificationService = notificationService;
        _exchangeRateserviceClient = exchangeRateserviceClient;
    }

    public async Task Process()
    {
        try
        {
            Console.WriteLine($"Start Importing.....");
            var rates = await _exchangeRateserviceClient.GetRates();

            await _dataService.SaveExchnageRatePollEvent(rates);

            var changedRates = await _dataService.GetRatesChangedComparedToMaterializedView(rates);
            if (!changedRates.DeletedRates.Any() && !changedRates.UpdatedRates.Any() && !changedRates.AddedRates.Any())
            {
                return;
            }

            var tasks = new List<Task>
            {
                _dataService.UpdateMaterializedView(changedRates),
                //_cacheService.AddToCache(changedRates),
                //_notificationService.Notify(changedRates),
            };
            await Task.WhenAll(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error:  { e.Message }");
            Console.WriteLine($"Error:  { e.StackTrace }");
        }
    }
}

