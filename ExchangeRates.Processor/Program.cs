using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRates.Processor.Services;
using Microsoft.Extensions.Configuration;
using ExchangeRates.Processor.Lib;
using ExchangeRates.Importer.Repos;
using ExchangeRates.Processor.Mappers;
using System;

namespace ExchangeRates.Importer
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
                    Console.WriteLine($"Conn str: {context.Configuration.GetSection("ConnectionStrings")}");
                    services.Configure<ConnectionStrings>(context.Configuration.GetSection("ConnectionStrings"));
                    services.AddTransient<IDBConnectionProvider, PGDBConnectionProvider>();
                    services.AddTransient<IExchangeRatesRepo, ExchangeRatesRepo>();

                    services.AddTransient<IExchangeRatesMapper, ExchangeRatesMapper>();

                    services.AddTransient<IExchangeRateProcessor, ExchangeRateProcessor>();
                    services.AddTransient<IExchangeRateDataService, ExchangeRateDataService>();
                    services.AddTransient<IExchangeRateCacheService, ExchangeRateCacheService>();
                    services.AddTransient<IExchangeRateServiceClient, ExchangeRateServiceClient>();
                    services.AddTransient<IEventNotificationService, EventNotificationService>();

                    services.AddTransient<IRatesImporter, RatesImporter>();

                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetRequiredService<IExchangeRateProcessor>().Process();
                });

            await host.RunConsoleAsync();
        }
    }
}
