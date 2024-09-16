using System;
using System.Collections.Generic;

namespace GettingStarted.Services;

public interface IOrderProfitEligibilityService
{
    public bool IsEligible(DateTime createdAt, List<string> statuses, string shippedStatusName);
}