using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class YearlySubscriptionPlan : ISubscriptionPlan
{
    public SubscriptionType Type => SubscriptionType.Yearly;
    public int GetRecommendedDuration() => 12;
    public bool HasFixedDuration() => true;
    public bool SupportsMultipleInstances() => false;

}
