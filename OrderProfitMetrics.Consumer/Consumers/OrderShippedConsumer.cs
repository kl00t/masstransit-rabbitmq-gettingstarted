namespace GettingStarted.Consumers;

using GettingStarted.Contracts;
using GettingStarted.Services;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

public class OrderShippedConsumer : IConsumer<Envelope>
{
    private readonly ILogger<OrderShippedConsumer> _logger;
    private readonly AdminHttpClient _adminClient;
    private readonly IOrderProfitEligibilityService _orderProfitEligibilityService;
    public OrderShippedConsumer(
        ILogger<OrderShippedConsumer> logger,
        AdminHttpClient adminClient,
        IOrderProfitEligibilityService orderProfitEligibilityService)
    {
        _logger = logger;
        _adminClient = adminClient;
        _orderProfitEligibilityService = orderProfitEligibilityService;
    }

    public async Task Consume(ConsumeContext<Envelope> context)
    {
        try
        {
            var orderCode = context.Message.Payload.Code;
            var order = await _adminClient.GetOrderAsync(new OrderRequest(orderCode));

            var statuses = order.StatusHistory.Select(s => s.Type).ToList();

            if (_orderProfitEligibilityService.IsEligible(order.CreatedAt, statuses, Status.SHIPPED_STATUS))
            {
                _logger.LogInformation($"Order {order.Code} eligible to push profit metrics");
            }
            else
            {
                _logger.LogInformation($"Order {order.Code} not eligible to push profit metrics");
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Order Shipped Consumer error");
            throw;
        }
    }
}
