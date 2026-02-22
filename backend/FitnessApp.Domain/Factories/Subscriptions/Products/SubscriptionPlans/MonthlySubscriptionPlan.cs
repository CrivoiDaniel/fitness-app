using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

public class MonthlySubscriptionPlan : ISubscriptionPlan
{
    public SubscriptionType Type => SubscriptionType.Monthly;
    public int GetRecommendedDuration() => 1;
    public bool HasFixedDuration() => true;
    public bool SupportsMultipleInstances() => false;

}
