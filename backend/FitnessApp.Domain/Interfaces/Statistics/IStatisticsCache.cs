using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Domain.Interfaces.Statistics;

/// <summary>
/// Cache management interface
/// </summary>
public interface IStatisticsCache
{
    void UpdateCache(IEnumerable<Subscription> subscriptions, IEnumerable<Payment> payments);
    bool IsExpired();
    void Clear();
    IEnumerable<Subscription> GetCachedSubscriptions();
    IEnumerable<Payment> GetCachedPayments();
}
