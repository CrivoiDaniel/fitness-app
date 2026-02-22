using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

public class IndividualSubscriptionPlan : ISubscriptionPlan
{
    public SubscriptionType Type => SubscriptionType.Individual;
    public int GetRecommendedDuration() => 0;
    public bool HasFixedDuration() => false;
    public bool SupportsMultipleInstances() => true;

}
