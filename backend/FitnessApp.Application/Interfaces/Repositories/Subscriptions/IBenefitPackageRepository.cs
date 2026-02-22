using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Application.Interfaces.Repositories.Subscriptions;

public interface IBenefitPackageRepository : IRepository<BenefitPackage>
{
    Task<IEnumerable<BenefitPackage>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<BenefitPackage?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<BenefitPackage?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BenefitPackage>> GetAllWithItemsAsync(CancellationToken cancellationToken = default);

}
