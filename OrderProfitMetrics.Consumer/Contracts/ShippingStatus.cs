using System;

namespace OrderProfitMetrics.Consumer.Contracts;

public static class ShippingStatus
{
    private static Random random = new();

    public static string GetRandom()
    {
        // Generate a random number (0 or 1)
        int status = random.Next(2);  // 0 or 1

        // Return "shipped" if 1, otherwise "not shipped"
        return status == 1 ? "shipped" : "not shipped";
    }
}
