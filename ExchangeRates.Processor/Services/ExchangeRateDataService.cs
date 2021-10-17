using ExchangeRates.Processor.Mappers;
using ExchangeRates.Common.Models;
using ExchangeRates.Common.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.Processor.Models;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateDataService
    {
        Task<ChangedRates> GetChangedExchangeRates(IEnumerable<ExchangeRate> rates);
        Task UpdateExchangeRates(ChangedRates changedRates);
        Task SaveExchangeRatePollEvent(ExchangeRateBatch rates);
    }

    public class ExchangeRateDataService : IExchangeRateDataService
    {
        private readonly IExchangeRatesRepo _repo;
        private readonly IExchangeRatesMapper _mapper;

        public ExchangeRateDataService(IExchangeRatesRepo repo, IExchangeRatesMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ChangedRates> GetChangedExchangeRates(IEnumerable<ExchangeRate> newRates)
        {
            var currentRates = await _repo.GetRates();

            var addedRates = GetAddedRates(currentRates, newRates);
            var updatedRates = GetUpdatedRates(currentRates, newRates);
            var deletedRates = GetDeletedRates(currentRates, newRates);

            return _mapper.MapChangedRates(addedRates, updatedRates, deletedRates);
        }

        private IEnumerable<ExchangeRate> GetDeletedRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates)
        {
            return currentRates.Where(newRate => !newRates.Any(currentRate => newRate.Code == currentRate.Code));
        }

        private IEnumerable<ExchangeRate> GetAddedRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates)
        {
            return newRates.Where(newRate => !currentRates.Any(currentRate => newRate.Code == currentRate.Code));
        }

        private IEnumerable<ExchangeRate> GetUpdatedRates(IEnumerable<ExchangeRate> previousRates, IEnumerable<ExchangeRate> currentRates)
        {
            if (!previousRates.Any())
                return new List<ExchangeRate>();

            var changedRates = new List<ExchangeRate>();
            var previousRatesDictionary = previousRates.ToDictionary(x => x.Code.ToLower(), x => Decimal.Round(x.Value, 4));

            foreach (var currentRate in currentRates)
            {
                var rateFound = previousRatesDictionary.TryGetValue(currentRate.Code.ToLower(), out var previousRate);
                if (rateFound && previousRate != Decimal.Round(currentRate.Value, 4))
                {
                    changedRates.Add(currentRate);
                }
            }

            return changedRates;
        }

        /// <summary>
        /// Save the batch with an updated date
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        public async Task SaveExchangeRatePollEvent(ExchangeRateBatch rates)
        {
            await _repo.SaveExchangeRateEvents(rates);
        }

        public async Task UpdateExchangeRates(ChangedRates changedRates)
        {
            if(changedRates.AddedRates.Any())
                await _repo.AddNewRates(changedRates.AddedRates);

            if (changedRates.UpdatedRates.Any())
                await _repo.UpdateRates(changedRates.UpdatedRates);

            if(changedRates.DeletedRates.Any())
                await _repo.DeleteRates(changedRates.DeletedRates);
        }
    }
}
