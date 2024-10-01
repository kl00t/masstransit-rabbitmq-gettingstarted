using System.Collections.Generic;
using System;

namespace OrderProfitMetrics.Consumer.Contracts;
public record OrderResponse(
    string Code,
    DateTime CreatedAt,
    int Profit,
    float ProfitPercentage,
    List<Status> StatusHistory);
