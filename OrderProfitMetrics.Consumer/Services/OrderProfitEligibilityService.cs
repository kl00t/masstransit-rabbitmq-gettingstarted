using System;
using System.Collections.Generic;
using System.Linq;

namespace GettingStarted.Services;

public class OrderProfitEligibilityService() : IOrderProfitEligibilityService
{
    public bool IsEligible(DateTime createdAt, List<string> statuses, string shippedStatusName)
    {
        if (statuses.Count(s => s == shippedStatusName) == 1)
        {
            return true;
        }

        return false;
    }
}
