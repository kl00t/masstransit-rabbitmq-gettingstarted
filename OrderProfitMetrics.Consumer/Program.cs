using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderProfitMetrics.Consumer.Extensions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace OrderProfitMetrics.Consumer;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(CreateHostBuilder(args).Build().Services.GetRequiredService<IConfiguration>())
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting up the service...");
            await CreateHostBuilder(args).Build().RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed!");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClients(hostContext.Configuration);
                services.AddMassTransitService(hostContext.Configuration);
                services.AddServices();
                services.AddHostedService<Worker>();
            });
}
