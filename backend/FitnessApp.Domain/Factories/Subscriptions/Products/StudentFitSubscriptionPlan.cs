using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class StudentFitSubscriptionPlan : ISubscriptionPlan
{
    public SubscriptionType Type => SubscriptionType.StudentFit;
    public int GetRecommendedDuration() => 1;
    public bool HasFixedDuration() => true;
    public bool SupportsMultipleInstances() => false;

}
