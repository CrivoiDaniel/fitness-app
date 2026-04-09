using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Observer;

namespace FitnessApp.Application.Observer;

/// <summary>
/// Observer Pattern: Subject (Publisher) Interface
/// Defineste metodele pentru gestionarea observatorilor.
/// </summary>
public interface ISubscriptionPublisher
{
    void Attach(ISubscriptionObserver observer);
    void Detach(ISubscriptionObserver observer);
    Task NotifyPurchaseAsync(Subscription subscription, decimal amount);
}
