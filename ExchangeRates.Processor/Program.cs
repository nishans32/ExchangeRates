using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Processor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((context, services) =>{
                    services.AddTransient<IExchangeRateProcessingService, ExchangeRateProcessingService>();
                    
                    var serviceProvider = services.BuildServiceProvider();
                    serviceProvider.GetRequiredService<IExchangeRateProcessingService>().Process();
                });

            await builder.RunConsoleAsync();
        }
    }
}
