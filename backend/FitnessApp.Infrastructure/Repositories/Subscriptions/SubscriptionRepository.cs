using System;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Subscriptions;

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Subscription>> GetByClientIdAsync(
        int clientId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.ClientId == clientId)
            .Include(s => s.SubscriptionPlan)
                .ThenInclude(sp => sp.BenefitPackage)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetActiveByClientIdAsync(
        int clientId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.ClientId == clientId && s.Status == SubscriptionStatus.Active)
            .Include(s => s.SubscriptionPlan)
                .ThenInclude(sp => sp.BenefitPackage)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> GetWithDetailsAsync(
        int id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Include(s => s.SubscriptionPlan)
                .ThenInclude(sp => sp.BenefitPackage)
                    .ThenInclude(bp => bp.BenefitPackageItems)
                        .ThenInclude(bpi => bpi.Benefit)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetByStatusAsync(
        SubscriptionStatus status, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Status == status)
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Include(s => s.SubscriptionPlan)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetExpiringAsync(
        int withinDays, 
        CancellationToken cancellationToken = default)
    {
        var targetDate = DateTime.UtcNow.AddDays(withinDays).Date;
        var today = DateTime.UtcNow.Date;

        return await _dbSet
            .Where(s => 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate.HasValue &&
                s.EndDate.Value.Date > today &&
                s.EndDate.Value.Date <= targetDate)
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Include(s => s.SubscriptionPlan)
            .OrderBy(s => s.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetExpiredAsync(
        CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        return await _dbSet
            .Where(s => 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate.HasValue &&
                s.EndDate.Value.Date < today)
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Include(s => s.SubscriptionPlan)
            .OrderBy(s => s.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasActiveSubscriptionAsync(
        int clientId, 
        int planId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(s => 
                s.ClientId == clientId &&
                s.SubscriptionPlanId == planId &&
                s.Status == SubscriptionStatus.Active,
                cancellationToken);
    }
    public async Task<List<Subscription>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Subscriptions
            .Include(s => s.Client)
                .ThenInclude(c => c.User)
            .Include(s => s.SubscriptionPlan)
                .ThenInclude(sp => sp.BenefitPackage)
            .Include(s => s.Payments)
            .ToListAsync(cancellationToken);
    }
}
