using OrderProfitMetrics.Consumer.Contracts;
using OrderProfitMetrics.Consumer.Serialization;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderProfitMetrics.Consumer;

public class AdminHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<OrderResponse> GetOrderAsync(OrderRequest orderRequest)
    {
        return new OrderResponse(
            orderRequest.Code,
            DateTime.UtcNow, 100, 100,
            [
                new(ShippingStatus.GetRandom())
            ]);
    }

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = new LowerSnakeCaseNamingPolicy()
    };
}