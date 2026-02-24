using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Domain.Interfaces.Statistics;

/// <summary>
/// Base interface for statistics calculators
/// </summary>
public interface IStatisticsCalculator<TResult>
{
    TResult Calculate(IEnumerable<Subscription> subscriptions, IEnumerable<Payment> payments);
}