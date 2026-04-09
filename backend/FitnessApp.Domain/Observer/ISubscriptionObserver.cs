using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Domain.Observer;

/// <summary>
/// Observer Pattern: Subscriber Interface
/// Defines the update method that concrete observers must implement.
/// </summary>
public interface ISubscriptionObserver
{
    /// <summary>
    /// Notifies the observer that a subscription has been purchased.
    /// </summary>
    /// <param name="subscription">The subscription details.</param>
    /// <param name="amount">The final amount paid.</param>
    Task OnPurchaseCompletedAsync(Subscription subscription, decimal amount);
}
