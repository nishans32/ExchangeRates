using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ExchangeRatesDto
{
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; }
}