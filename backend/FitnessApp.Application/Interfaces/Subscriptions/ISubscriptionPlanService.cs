using System;
using FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface ISubscriptionPlanService
{
    Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlanDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlanDto>> GetByTypeAsync(SubscriptionType type, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto, CancellationToken cancellationToken = default);
    Task<SubscriptionPlanDto> UpdateAsync(int id, UpdateSubscriptionPlanDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}