using GettingStarted.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace GettingStarted;

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
                services.AddMassTransitService(hostContext.Configuration, useInMemory: false);
                services.AddServices();
                // This will be used to create items on the queue.
                services.AddHostedService<Worker>();
            });
}
