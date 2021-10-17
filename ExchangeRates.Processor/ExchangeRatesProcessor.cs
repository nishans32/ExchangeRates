using ExchangeRates.Processor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IExchangeRateProcessor
{
    Task Process();
}

public class ExchangeRateProcessor : IExchangeRateProcessor
{
    private readonly IExchangeRateCacheService _cacheService;
    private readonly IEventNotificationService _notificationService;
    private readonly IExchangeRateDataService _dataService ;
    private readonly IRatesImporter _ratesImporter;

    public ExchangeRateProcessor(IExchangeRateDataService dataService, IExchangeRateCacheService cacheService, IEventNotificationService notificationService, IRatesImporter ratesImporter)
    {
        _dataService = dataService;
        _cacheService = cacheService;
        _notificationService = notificationService;
        _ratesImporter = ratesImporter;
    }

    public async Task Process()
    {
        try
        {
            Console.WriteLine($"Start Importing.....");

            var exchangeRatesBatch = await _ratesImporter.Import();

            await _dataService.SaveExchangeRatePollEvent(exchangeRatesBatch);

            var changedRates = await _dataService.GetChangedExchangeRates(exchangeRatesBatch.Rates);
            if (!HasRatesChanged(changedRates))
            {
                Console.WriteLine($"No changes to rates. Quitting...");
                return;
            }

            await Task.WhenAll(
                _dataService.UpdateExchangeRates(changedRates));
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error:  { e.Message }");
            Console.WriteLine($"Error:  { e.StackTrace }");
        }
    }

    private bool HasRatesChanged(ExchangeRates.Processor.Models.ChangedRates changedRates)
    {
        return  changedRates.DeletedRates.Any() || 
                changedRates.UpdatedRates.Any() || 
                changedRates.AddedRates.Any();
    }
}

