using System.Threading.Tasks;
using FitnessApp.Application.Services;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Observer;

namespace FitnessApp.Application.Observer;

/// <summary>
/// Observer Pattern: Concrete Observer
/// Actualizează Singleton-ul de statistici imediat ce o plată este procesată.
/// </summary>
public class StatisticsUpdateObserver : ISubscriptionObserver
{
    public Task OnPurchaseCompletedAsync(Subscription subscription, decimal amount)
    {
        Console.WriteLine("[StatisticsObserver] Order detected: {0} MDL. Triggering status recalculation.", amount);
        
        // În viața reală, am putea adăuga doar tranzacția nouă în cache.
        // Pentru acest lab, invalidăm cache-ul pentru a forța reîncărcarea datelor proaspete din DB.
        SubscriptionStatisticsManager.Instance.ClearCache();
        
        return Task.CompletedTask;
    }
}
