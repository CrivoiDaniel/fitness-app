using System;
using FitnessApp.Application.DTOs.Subscriptions.Benefit;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface IBenefitService
{
    Task<IEnumerable<BenefitDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<BenefitDto>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<BenefitDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<BenefitDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<BenefitDto> CreateAsync(CreateBenefitDto dto, CancellationToken cancellationToken = default);
    Task<BenefitDto> UpdateAsync(int id, UpdateBenefitDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

}
