namespace GettingStarted.Contracts;
using System.Collections.Generic;
using System;

public record OrderResponse(
    string Code,
    DateTime CreatedAt,
    int Profit,
    float ProfitPercentage,
    List<Status> StatusHistory);
