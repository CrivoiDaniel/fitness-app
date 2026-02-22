using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

//Defines LOGIC for subscription plan types
public interface ISubscriptionPlan
{
    SubscriptionType Type { get; }
    int GetRecommendedDuration();
    bool HasFixedDuration();
    bool SupportsMultipleInstances();

}
