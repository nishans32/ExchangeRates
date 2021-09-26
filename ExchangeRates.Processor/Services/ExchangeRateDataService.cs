using ExchangeRates.Processor.Mappers;
using ExchangeRates.Processor.Models;
using ExchangeRates.Processor.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRates.Processor.Services
{
    public interface IExchangeRateDataService
    {
        Task<ChangedRates> GetRatesChangedComparedToMaterializedView(ExchangeRatesDto rates);
        Task UpdateMaterializedView(ChangedRates changedRates);
        Task SaveExchnageRatePollEvent(ExchangeRatesDto rates);
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

        public async Task<ChangedRates> GetRatesChangedComparedToMaterializedView(ExchangeRatesDto newRatesDto)
        {
            var newRates = _mapper.MapToExchangeRate(newRatesDto);
            var currentRates = await _repo.GetRates();

            var addedRates = GetAddedRates(currentRates, newRates);
            var updatedRates = GetUpdatedRates(currentRates, newRates);
            var deletedRates = GetDeletedRates(currentRates, newRates);

            return new ChangedRates
            {
                AddedRates = addedRates,
                UpdatedRates = updatedRates,
                DeletedRates = deletedRates
            };
        }

        public IEnumerable<ExchangeRate> GetDeletedRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates)
        {
            return currentRates.Where(newRate => !newRates.Any(currentRate => newRate.Code == currentRate.Code));
        }

        public IEnumerable<ExchangeRate> GetAddedRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates)
        {
            return newRates.Where(newRate => !currentRates.Any(currentRate => newRate.Code == currentRate.Code));
        }

        private IEnumerable<ExchangeRate> GetUpdatedRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates)
        {
            if (!currentRates.Any())
                return new List<ExchangeRate>();

            var changedRates = new List<ExchangeRate>();
            var currentRateDict = currentRates.ToDictionary(x => x.Code.ToLower(), x => x.Value);

            foreach (var newRate in newRates)
            {
                var rateFound = currentRateDict.TryGetValue(newRate.Code.ToLower(), out var currentRate);
                if (rateFound && currentRate != newRate.Value)
                {
                    changedRates.Add(newRate);
                }
            }

            return changedRates;
        }

        /// <summary>
        /// Save the batch with an updated date
        /// </summary>
        /// <param name="rates"></param>
        /// <returns></returns>
        public async Task SaveExchnageRatePollEvent(ExchangeRatesDto rates)
        {
            var ratesToSave = _mapper.MapToExchangeRateEvent(rates);
            await _repo.SaveExchangeRateEvents(ratesToSave);

        }

        public async Task UpdateMaterializedView(ChangedRates changedRates)
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
