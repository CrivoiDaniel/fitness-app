using System;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Subscriptions;

public class BenefitRepository : Repository<Benefit> ,IBenefitRepository
{
    public BenefitRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Benefit>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Benefit?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b => b.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(b => b.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(b => b.DisplayName == displayName, cancellationToken);
    }

}
