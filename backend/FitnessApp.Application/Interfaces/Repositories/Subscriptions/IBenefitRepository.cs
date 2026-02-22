using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Application.Interfaces.Repositories.Subscriptions;

/// <summary>
/// Repository interface for Benefit entity
/// </summary>
public interface IBenefitRepository : IRepository<Benefit>
{
    Task<IEnumerable<Benefit>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Benefit?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByDisplayNameAsync(string displayName, CancellationToken cancellationToken = default);
}