using System;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Subscriptions;

public class BenefitPackageRepository : Repository<BenefitPackage>, IBenefitPackageRepository
{
    public BenefitPackageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BenefitPackage>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(bp => bp.IsActive)
            .OrderBy(bp => bp.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<BenefitPackage?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(bp => bp.Name == name, cancellationToken);
    }

    public async Task<BenefitPackage?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(bp => bp.BenefitPackageItems)
                .ThenInclude(bpi => bpi.Benefit)
            .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<BenefitPackage>> GetAllWithItemsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(bp => bp.BenefitPackageItems)
                .ThenInclude(bpi => bpi.Benefit)
            .OrderBy(bp => bp.Name)
            .ToListAsync(cancellationToken);
    }
}
