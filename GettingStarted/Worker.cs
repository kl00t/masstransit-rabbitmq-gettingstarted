using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GettingStarted;

public class Worker : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<Worker> _logger;

    public Worker(IBus bus, ILogger<Worker> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Worker {nameof(Worker)} started.");
        while (!cancellationToken.IsCancellationRequested)
        {
            var orderCode = Guid.NewGuid().ToString();
            var orderShipped = new OrderShippedEvent(orderCode);
            var envelope = new Contracts.Envelope(orderShipped);
            await _bus.Publish(envelope, cancellationToken);
            //await _bus.Publish(new Contracts.GettingStarted { Value = $"The time is {DateTimeOffset.Now}" }, cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
    }
}
