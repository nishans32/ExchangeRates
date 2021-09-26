﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRates.Importer.Models
{
    public class ExchangeRate
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
    public class ExchangeRateEvent
    {
        public Guid BatchId { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
    }
}
