using System;
using System.Collections.Generic;

namespace OrderProfitMetrics.Consumer.Services;

public interface IOrderProfitEligibilityService
{
    public bool IsEligible(DateTime createdAt, List<string> statuses, string shippedStatusName);
}