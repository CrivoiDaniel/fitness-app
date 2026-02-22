using System;
using FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Application.Services.Subscriptions;

public class BenefitPackageService : IBenefitPackageService
{
    private readonly IBenefitPackageRepository _packageRepository;
    private readonly IBenefitRepository _benefitRepository;

    public BenefitPackageService(
        IBenefitPackageRepository packageRepository,
        IBenefitRepository benefitRepository)
    {
        _packageRepository = packageRepository;
        _benefitRepository = benefitRepository;
    }

    public async Task<IEnumerable<BenefitPackageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.GetAllWithItemsAsync(cancellationToken);
        return packages.Select(MapToDto);
    }

    public async Task<IEnumerable<BenefitPackageDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var packages = await _packageRepository.GetActiveAsync(cancellationToken);
        return packages.Select(MapToDto);
    }

    public async Task<BenefitPackageDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        return package == null ? null : MapToDto(package);
    }

    public async Task<BenefitPackageDto?> GetWithItemsAsync(int id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetWithItemsAsync(id, cancellationToken);
        return package == null ? null : MapToDto(package);
    }

    public async Task<BenefitPackageDto> CreateAsync(CreateBenefitPackageDto dto, CancellationToken cancellationToken = default)
    {
        // Verifică duplicate name
        var existingPackage = await _packageRepository.GetByNameAsync(dto.Name, cancellationToken);
        if (existingPackage != null)
        {
            throw new InvalidOperationException($"Benefit package with name '{dto.Name}' already exists.");
        }

        // Verifică că toate benefits există
        foreach (var item in dto.Items)
        {
            var benefit = await _benefitRepository.GetByIdAsync(item.BenefitId, cancellationToken);
            if (benefit == null)
            {
                throw new InvalidOperationException($"Benefit with ID {item.BenefitId} not found.");
            }
        }

        // Creează package (folosește constructor)
        var package = new BenefitPackage(dto.Name, dto.ScheduleWeekday, dto.ScheduleWeekend);

        // Adaugă items
        foreach (var itemDto in dto.Items)
        {
            var item = new BenefitPackageItem(package.Id, itemDto.BenefitId, itemDto.Value);
            package.BenefitPackageItems.Add(item);
        }

        var created = await _packageRepository.AddAsync(package, cancellationToken);
        return MapToDto(created);
    }

    public async Task<BenefitPackageDto> UpdateAsync(int id, UpdateBenefitPackageDto dto, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetWithItemsAsync(id, cancellationToken);
        if (package == null)
        {
            throw new KeyNotFoundException($"Benefit package with ID {id} not found.");
        }

        // Verifică duplicate name (exclude current package)
        var existingPackage = await _packageRepository.GetByNameAsync(dto.Name, cancellationToken);
        if (existingPackage != null && existingPackage.Id != id)
        {
            throw new InvalidOperationException($"Benefit package with name '{dto.Name}' already exists.");
        }

        // Verifică că toate benefits există
        foreach (var item in dto.Items)
        {
            var benefit = await _benefitRepository.GetByIdAsync(item.BenefitId, cancellationToken);
            if (benefit == null)
            {
                throw new InvalidOperationException($"Benefit with ID {item.BenefitId} not found.");
            }
        }

        // Update basic info (folosește metodele din entity dacă există)
        package.UpdateName(dto.Name);
        package.UpdateSchedule(dto.ScheduleWeekday, dto.ScheduleWeekend);

        if (dto.IsActive)
            package.Activate();
        else
            package.Deactivate();

        // Update items - clear și re-add
        package.BenefitPackageItems.Clear();
        
        foreach (var itemDto in dto.Items)
        {
            var item = new BenefitPackageItem(package.Id, itemDto.BenefitId, itemDto.Value);
            package.BenefitPackageItems.Add(item);
        }

        await _packageRepository.UpdateAsync(package, cancellationToken);
        return MapToDto(package);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var package = await _packageRepository.GetByIdAsync(id, cancellationToken);
        if (package == null)
        {
            throw new KeyNotFoundException($"Benefit package with ID {id} not found.");
        }

        await _packageRepository.DeleteAsync(package, cancellationToken);
    }

    // Private mapping helper
    private static BenefitPackageDto MapToDto(BenefitPackage package)
    {
        return new BenefitPackageDto
        {
            Id = package.Id,
            Name = package.Name,
            ScheduleWeekday = package.ScheduleWeekday,
            ScheduleWeekend = package.ScheduleWeekend,
            IsActive = package.IsActive,
            Items = package.BenefitPackageItems?.Select(item => new BenefitPackageItemDto
            {
                BenefitId = item.BenefitId,
                BenefitName = item.Benefit?.Name ?? string.Empty,
                BenefitDisplayName = item.Benefit?.DisplayName ?? string.Empty,
                Value = item.Value
            }).ToList() ?? new List<BenefitPackageItemDto>(),
            CreatedAt = package.CreatedAt,
            UpdatedAt = package.UpdatedAt
        };
    }
}