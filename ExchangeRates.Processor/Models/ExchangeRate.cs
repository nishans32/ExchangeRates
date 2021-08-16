using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Processor.Models
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}
