using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Domain.Interfaces.Statistics;

/// <summary>
/// Trend calculation interface
/// </summary>
public interface ITrendCalculator
{
    decimal CalculateGrowthRate(IEnumerable<Subscription> subscriptions);
    decimal CalculateChurnRate(IEnumerable<Subscription> subscriptions);
}