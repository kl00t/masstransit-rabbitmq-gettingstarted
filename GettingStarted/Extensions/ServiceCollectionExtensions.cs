using GettingStarted.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace GettingStarted.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderProfitEligibilityService, OrderProfitEligibilityService>();
    }

    public static void AddMassTransitService(this IServiceCollection services, IConfiguration configuration, bool useInMemory)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            // By default, sagas are in-memory, but should be changed to a durable
            // saga repository.
            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddConsumers(entryAssembly);
            x.AddSagaStateMachines(entryAssembly);
            x.AddSagas(entryAssembly);
            x.AddActivities(entryAssembly);

            if (useInMemory)
            {
                // Use In Memory version.
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                // Use if Docker is configured. See ReadMe
                x.UsingRabbitMq((context, cfg) =>
                {
                    var config = configuration.GetSection("RabbitMq");

                    cfg.Host(new Uri($"amqps://{config["Host"]}:{config["Port"]}"), config["VirtualHost"], h =>
                    {
                        h.Username(config["Username"]);
                        h.Password(config["Password"]);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            }
        });
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<AdminHttpClient>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", configuration["AdminApiKey"]);
            client.BaseAddress = new Uri(configuration["AdminUrl"]);
        });
    }
}
