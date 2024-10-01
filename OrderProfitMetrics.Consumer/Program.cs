using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProfitMetrics.Consumer.Extensions;
using System.Threading.Tasks;

namespace OrderProfitMetrics.Consumer;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClients(hostContext.Configuration);
                services.AddMassTransitService(hostContext.Configuration);
                services.AddServices();
                services.AddHostedService<Worker>();
            });
}
