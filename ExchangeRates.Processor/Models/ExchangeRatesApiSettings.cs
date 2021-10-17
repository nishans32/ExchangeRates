namespace ExchangeRates.Processor.Services
{
    public class ExchangeRatesApiSettings
    {
        public string AppId { get; set; }
        public string RatesUrl { get; set; }
        public string RatesNamesUrl { get; set; }
    }

    public class AppSettings
    {
        public string TempDirectory { get; set; }
    }
}