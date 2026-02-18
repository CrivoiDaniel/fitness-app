using System;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class SubscriptionPackageFactoryProvider
{
    public static ISubscriptionPackageFactory GetFactory(SubscriptionType type)
    {
        return type switch
        {
            SubscriptionType.Monthly => new MonthlyPackageFactory(),
            SubscriptionType.Quarterly => new QuarterlyPackageFactory(),
            SubscriptionType.Yearly => new YearlyPackageFactory(),
            SubscriptionType.Individual => new IndividualPackageFactory(),
            SubscriptionType.StudentFit => new StudentFitPackageFactory(),
            _ => throw new ArgumentException($"Unknown subscription type: {type}")
        };
    }

    public static IEnumerable<ISubscriptionPackageFactory> GetAllFactories()
    {
        return new List<ISubscriptionPackageFactory>
        {
            new MonthlyPackageFactory(),
            new QuarterlyPackageFactory(),
            new YearlyPackageFactory(),
            new IndividualPackageFactory(),
            new StudentFitPackageFactory()
        };
    }
}
