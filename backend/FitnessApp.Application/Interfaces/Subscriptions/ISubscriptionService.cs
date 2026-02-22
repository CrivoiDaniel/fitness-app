using System;
using FitnessApp.Application.DTOs.Subscriptions.Subscription;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionDto>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionDto>> GetActiveByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionDto>> GetByStatusAsync(SubscriptionStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionDto>> GetExpiringAsync(int withinDays, CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> UpdateAsync(int id, UpdateSubscriptionDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> CancelAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionDto> RenewAsync(int id, CancellationToken cancellationToken = default);
}