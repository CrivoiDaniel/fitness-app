using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Domain.Interfaces.Statistics;

namespace FitnessApp.Application.Services.Statistics;

public class MetricsCalculator : IMetricsCalculator
{
    public int CountByStatus(IEnumerable<Subscription> subscriptions, SubscriptionStatus status)
    {
        return subscriptions.Count(s => s.Status == status);
    }
    
    public decimal CalculatePercentage(int count, int total)
    {
        if (total == 0) return 0;
        return (decimal)count / total * 100;
    }
    
    public Dictionary<string, int> GroupByType(IEnumerable<Subscription> subscriptions)
    {
        return subscriptions
            .GroupBy(s => s.SubscriptionPlan?.Type.ToString() ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());
    }
}