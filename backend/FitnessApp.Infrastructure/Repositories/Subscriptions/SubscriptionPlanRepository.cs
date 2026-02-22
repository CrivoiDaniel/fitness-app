using System;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Subscriptions;

public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sp => sp.IsActive)
            .Include(sp => sp.BenefitPackage)
            .OrderBy(sp => sp.Type)
            .ThenBy(sp => sp.Price)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetByTypeAsync(
        SubscriptionType type,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sp => sp.Type == type && sp.IsActive)
            .Include(sp => sp.BenefitPackage)
            .OrderBy(sp => sp.Price)
            .ToListAsync(cancellationToken);
    }

    public async Task<SubscriptionPlan?> GetWithBenefitPackageAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(sp => sp.BenefitPackage)
                .ThenInclude(bp => bp.BenefitPackageItems)
                    .ThenInclude(bpi => bpi.Benefit)
            .FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetAllWithBenefitPackagesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(sp => sp.BenefitPackage)
                .ThenInclude(bp => bp.BenefitPackageItems)
                    .ThenInclude(bpi => bpi.Benefit)
            .OrderBy(sp => sp.Type)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetRecurringPlansAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sp => sp.IsRecurring && sp.IsActive)
            .Include(sp => sp.BenefitPackage)
            .OrderBy(sp => sp.Type)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<SubscriptionPlan>> GetPlansWithInstallmentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(sp => sp.AllowInstallments && sp.IsActive)
            .Include(sp => sp.BenefitPackage)
            .OrderBy(sp => sp.Type)
            .ToListAsync(cancellationToken);
    }


}