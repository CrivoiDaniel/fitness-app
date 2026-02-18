using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class QuarterlySubscriptionPlan : ISubscriptionPlan
{
    public SubscriptionType Type => SubscriptionType.Quarterly;
    public int GetRecommendedDuration() => 3;
    public bool HasFixedDuration() => true;
    public bool SupportsMultipleInstances() => false;

}
