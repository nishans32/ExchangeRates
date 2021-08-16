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
        Task<IEnumerable<ExchangeRate>> GetChangedRates(ExchangeRatesDto rates);
        Task SaveLastUpdatedDate(object rates);
        Task SaveNewRates(object changedRates);
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

        public async Task<IEnumerable<ExchangeRate>> GetChangedRates(ExchangeRatesDto newRates)
        {
            var newMappedRates = _mapper.MapRatesToModel(newRates);
            var currentRates = await _repo.GetRates();

            return CompareRates(currentRates, newMappedRates);
        }


        public Task SaveLastUpdatedDate(object rates)
        {
            throw new NotImplementedException();
        }

        public Task SaveNewRates(object changedRates)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compares the two different lists of exchange rates and returns ones changed from the current list. 
        /// Probably needs to be pulled out and maybe use an IEquilityComaprer approach
        /// </summary>
        /// <param name="currentRates"></param>
        /// <param name="newRates"></param>
        /// <returns></returns>
        private IEnumerable<ExchangeRate> CompareRates(IEnumerable<ExchangeRate> currentRates, IEnumerable<ExchangeRate> newRates )
        {
            if (!currentRates.Any())
                return newRates;

            var changedRates = new List<ExchangeRate>();
            var currentRateDict = currentRates.ToDictionary(x => x.Code.ToLower(), x => x.Value);
            
            foreach (var newRate in newRates)
            {
                var rateFound = currentRateDict.TryGetValue(newRate.Code.ToLower(), out var currentRate);
                if (!rateFound || currentRate != newRate.Value)
                {
                    changedRates.Add(newRate);
                }
            }

            return changedRates;
        }
    }
}
