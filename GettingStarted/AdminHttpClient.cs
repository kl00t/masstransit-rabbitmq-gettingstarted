using GettingStarted.Contracts;
using GettingStarted.Serialization;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GettingStarted;

public class AdminHttpClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<OrderResponse?> GetOrderAsync(OrderRequest orderRequest)
    {
        return new OrderResponse(
            orderRequest.Code,
            DateTime.UtcNow, 100, 100,
            [
                new(ShippingStatus.GetRandom())
            ]);

        //var response = await _httpClient.GetAsync($"/a/admin/api/v2/orders/{orderRequest.Code}");

        //if (!response.IsSuccessStatusCode)
        //{
        //    var errorContent = await response.Content.ReadAsStringAsync();
        //    throw new Exception(errorContent);
        //}

        //return await response.Content.ReadFromJsonAsync<OrderResponse>(_serializerOptions);
    }

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = new LowerSnakeCaseNamingPolicy()
    };
}