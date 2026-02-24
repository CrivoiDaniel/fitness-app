using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Interfaces.Statistics;

/// <summary>
/// Metrics calculation interface
/// </summary>
public interface IMetricsCalculator
{
    int CountByStatus(IEnumerable<Subscription> subscriptions, SubscriptionStatus status);
    decimal CalculatePercentage(int count, int total);
    Dictionary<string, int> GroupByType(IEnumerable<Subscription> subscriptions);
}