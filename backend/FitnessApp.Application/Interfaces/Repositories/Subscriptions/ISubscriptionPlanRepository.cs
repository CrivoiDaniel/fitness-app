using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Repositories.Subscriptions;

public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    Task<IEnumerable<SubscriptionPlan>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetByTypeAsync(SubscriptionType type, CancellationToken cancellationToken = default);
    Task<SubscriptionPlan?> GetWithBenefitPackageAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetAllWithBenefitPackagesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetRecurringPlansAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SubscriptionPlan>> GetPlansWithInstallmentsAsync(CancellationToken cancellationToken = default);
}