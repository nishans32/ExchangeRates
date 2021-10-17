using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Common.Models
{
    public class ExchangeRateBatch
    {
        public Guid BatchId { get; set; }
        public IEnumerable<ExchangeRate> Rates { get; set; }
    }
}
