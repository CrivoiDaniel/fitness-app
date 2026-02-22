using System;
using FitnessApp.Application.DTOs.Subscriptions.Benefit;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Application.Services.Subscriptions;

public class BenefitService : IBenefitService
{
    private readonly IBenefitRepository _benefitRepository;

    public BenefitService(IBenefitRepository benefitRepository)
    {
        _benefitRepository = benefitRepository;
    }

    public async Task<IEnumerable<BenefitDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var benefits = await _benefitRepository.GetAllAsync(cancellationToken);
        return benefits.Select(MapToDto);
    }

    public async Task<IEnumerable<BenefitDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var benefits = await _benefitRepository.GetActiveAsync(cancellationToken);
        return benefits.Select(MapToDto);
    }

    public async Task<BenefitDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var benefit = await _benefitRepository.GetByIdAsync(id, cancellationToken);
        return benefit == null ? null : MapToDto(benefit);
    }

    public async Task<BenefitDto?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var benefit = await _benefitRepository.GetByNameAsync(name, cancellationToken);
        return benefit == null ? null : MapToDto(benefit);
    }

    public async Task<BenefitDto> CreateAsync(CreateBenefitDto dto, CancellationToken cancellationToken = default)
    {
        // Business validation - verifică duplicate
        if (await _benefitRepository.ExistsByNameAsync(dto.Name, cancellationToken))
        {
            throw new InvalidOperationException($"Benefit with name '{dto.Name}' already exists.");
        }

        if (await _benefitRepository.ExistsByDisplayNameAsync(dto.DisplayName, cancellationToken))
        {
            throw new InvalidOperationException($"Benefit with display name '{dto.DisplayName}' already exists.");
        }

        var benefit = new Benefit(dto.Name, dto.DisplayName, dto.Description);
        var created = await _benefitRepository.AddAsync(benefit, cancellationToken);
        return MapToDto(created);
    }

    public async Task<BenefitDto> UpdateAsync(int id, UpdateBenefitDto dto, CancellationToken cancellationToken = default)
    {
        var benefit = await _benefitRepository.GetByIdAsync(id, cancellationToken);
        if (benefit == null)
        {
            throw new KeyNotFoundException($"Benefit with ID {id} not found.");
        }

        benefit.UpdateDisplayName(dto.DisplayName);
        benefit.UpdateDescription(dto.Description);
        
        if (dto.IsActive)
            benefit.Activate();
        else
            benefit.Deactivate();

        await _benefitRepository.UpdateAsync(benefit, cancellationToken);
        return MapToDto(benefit);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var benefit = await _benefitRepository.GetByIdAsync(id, cancellationToken);
        if (benefit == null)
        {
            throw new KeyNotFoundException($"Benefit with ID {id} not found.");
        }

        await _benefitRepository.DeleteAsync(benefit, cancellationToken);
    }

    // Private mapping helper
    private static BenefitDto MapToDto(Benefit benefit)
    {
        return new BenefitDto
        {
            Id = benefit.Id,
            Name = benefit.Name,
            DisplayName = benefit.DisplayName,
            Description = benefit.Description,
            IsActive = benefit.IsActive,
            CreatedAt = benefit.CreatedAt,
            UpdatedAt = benefit.UpdatedAt
        };
    }
}
