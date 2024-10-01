using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProfitMetrics.Consumer;
using OrderProfitMetrics.Consumer.Consumers;
using OrderProfitMetrics.Consumer.Serialization;
using OrderProfitMetrics.Consumer.Services;
using System;
using System.Reflection;

namespace OrderProfitMetrics.Consumer.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderProfitEligibilityService, OrderProfitEligibilityService>();
    }

    public static void AddMassTransitService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly());

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

                cfg.ReceiveEndpoint(config["OrderShippedQueue"], e =>
                {
                    e.ClearSerialization();
                    e.UseRawJsonSerializer();
                    e.UseRawJsonDeserializer();

                    // disable the default topic binding
                    e.ConfigureConsumeTopology = false;
                    e.PublishFaults = false;

                    e.Bind(config["StatsmaniaExchange"], x =>
                    {
                        x.ExchangeType = "topic";
                        x.RoutingKey = "order.shipped";
                    });

                    e.ConfigureConsumer<OrderShippedConsumer>(context);
                });

                cfg.UseMessageRetry(r => r.Exponential(
                    int.Parse(config["RetryCount"]),
                    TimeSpan.FromSeconds(int.Parse(config["RetryMinIntervalSeconds"])),
                    TimeSpan.FromSeconds(int.Parse(config["RetryMaxIntervalSeconds"])),
                    TimeSpan.FromSeconds(int.Parse(config["RetryStartingIntervalSeconds"]))
                ));

                cfg.ConfigureJsonSerializerOptions(options =>
                {
                    options.PropertyNamingPolicy = new LowerSnakeCaseNamingPolicy();
                    return options;
                });
            });
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
