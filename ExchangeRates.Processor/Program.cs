using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRates.Processor.Services;
using Microsoft.Extensions.Configuration;
using ExchangeRates.Processor.Lib;


namespace ExchangeRates.Processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(configuration=> 
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    configuration.Build();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();

                    services.Configure<ExchangeRatesApiSettings>(context.Configuration.GetSection("ExchangeRatesApiSettings"));
                    services.AddHttpClient<IHttpClientService, HttpClientService>();
                    services.AddTransient<IHttpClientService, HttpClientService>();
                    services.AddTransient<IJsonSerializer, JsonSerializerService>();

                    services.AddTransient<IExchangeRateImporter, ExchangeRateImporter>();
                    services.AddTransient<IExchangeRateDataService, ExchangeRateDataService>();
                    services.AddTransient<IExchangeRateCacheService, ExchangeRateCacheService>();
                    services.AddTransient<IExchangeRateServiceClient, ExchangeRateServiceClient>();
                    services.AddTransient<IEventNotificationService, EventNotificationService>();

                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetRequiredService<IExchangeRateImporter>().Process();
                });

            await host.RunConsoleAsync();
        }
    }
}
