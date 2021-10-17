using ExchangeRates.Common.Models;
using System.Collections.Generic;

namespace ExchangeRates.Processor.Models
{
    public class ChangedRates
    {
        public IEnumerable<ExchangeRate> AddedRates { get; set; }
        public IEnumerable<ExchangeRate> UpdatedRates { get; set; }
        public IEnumerable<ExchangeRate> DeletedRates { get; set; }

    }
}
