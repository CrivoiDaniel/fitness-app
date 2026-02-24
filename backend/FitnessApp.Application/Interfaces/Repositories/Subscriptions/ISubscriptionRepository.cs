using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Repositories.Subscriptions;

public interface ISubscriptionRepository : IRepository<Subscription>
{
    Task<IEnumerable<Subscription>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetActiveByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<Subscription?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetByStatusAsync(SubscriptionStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetExpiringAsync(int withinDays, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> GetExpiredAsync(CancellationToken cancellationToken = default);
    Task<bool> HasActiveSubscriptionAsync(int clientId, int planId, CancellationToken cancellationToken = default);
    Task<List<Subscription>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
}
