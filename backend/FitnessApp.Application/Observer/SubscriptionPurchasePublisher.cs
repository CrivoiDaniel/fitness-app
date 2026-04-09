using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Observer;

namespace FitnessApp.Application.Observer;

/// <summary>
/// Observer Pattern: Concrete Subject
/// Gestionează lista de observatori și îi notifică la finalizarea unei achiziții.
/// </summary>
public class SubscriptionPurchasePublisher : ISubscriptionPublisher
{
    private readonly List<ISubscriptionObserver> _observers = new();

    public SubscriptionPurchasePublisher(IEnumerable<ISubscriptionObserver> manualObservers)
    {
        // Add observers registered in DI automatically
        foreach (var observer in manualObservers)
        {
            _observers.Add(observer);
        }
    }

    public void Attach(ISubscriptionObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(ISubscriptionObserver observer)
    {
        _observers.Remove(observer);
    }

    public async Task NotifyPurchaseAsync(Subscription subscription, decimal amount)
    {
        Console.WriteLine("[Publisher] Notifying {0} observers about purchase of subscription {1}", 
            _observers.Count, subscription.Id);

        foreach (var observer in _observers)
        {
            await observer.OnPurchaseCompletedAsync(subscription, amount);
        }
    }
}
