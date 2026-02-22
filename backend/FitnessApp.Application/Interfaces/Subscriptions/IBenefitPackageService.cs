using System;
using FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface IBenefitPackageService
{
    Task<IEnumerable<BenefitPackageDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BenefitPackageDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<BenefitPackageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BenefitPackageDto?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<BenefitPackageDto> CreateAsync(CreateBenefitPackageDto dto, CancellationToken cancellationToken = default);
    Task<BenefitPackageDto> UpdateAsync(int id, UpdateBenefitPackageDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
