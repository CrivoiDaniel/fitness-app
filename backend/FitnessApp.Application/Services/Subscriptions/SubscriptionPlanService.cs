using System;
using FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;
using FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Services.Subscriptions;

public class SubscriptionPlanService : ISubscriptionPlanService
{
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly IBenefitPackageRepository _packageRepository;

    public SubscriptionPlanService(
        ISubscriptionPlanRepository planRepository,
        IBenefitPackageRepository packageRepository)
    {
        _planRepository = planRepository;
        _packageRepository = packageRepository;
    }

    public async Task<IEnumerable<SubscriptionPlanDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _planRepository.GetAllWithBenefitPackagesAsync(cancellationToken);
        return plans.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionPlanDto>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var plans = await _planRepository.GetActiveAsync(cancellationToken);
        return plans.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionPlanDto>> GetByTypeAsync(SubscriptionType type, CancellationToken cancellationToken = default)
    {
        var plans = await _planRepository.GetByTypeAsync(type, cancellationToken);
        return plans.Select(MapToDto);
    }

    public async Task<SubscriptionPlanDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
        return plan == null ? null : MapToDto(plan);
    }

    public async Task<SubscriptionPlanDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetWithBenefitPackageAsync(id, cancellationToken);
        return plan == null ? null : MapToDto(plan);
    }

    public async Task<SubscriptionPlanDto> CreateAsync(CreateSubscriptionPlanDto dto, CancellationToken cancellationToken = default)
    {
        // Verifică că benefit package există
        var package = await _packageRepository.GetByIdAsync(dto.BenefitPackageId, cancellationToken);
        if (package == null)
        {
            throw new InvalidOperationException($"Benefit package with ID {dto.BenefitPackageId} not found.");
        }

        if (!package.IsActive)
        {
            throw new InvalidOperationException($"Benefit package with ID {dto.BenefitPackageId} is not active.");
        }

        // Creează plan (folosește constructor)
        var plan = new SubscriptionPlan(
            dto.Type,
            dto.DurationInMonths,
            dto.Price,
            dto.BenefitPackageId,
            dto.IsRecurring,
            dto.AllowInstallments,
            dto.MaxInstallments);

        var created = await _planRepository.AddAsync(plan, cancellationToken);
        return MapToDto(created);
    }

    public async Task<SubscriptionPlanDto> UpdateAsync(int id, UpdateSubscriptionPlanDto dto, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
        if (plan == null)
        {
            throw new KeyNotFoundException($"Subscription plan with ID {id} not found.");
        }

        // Verifică că benefit package există
        var package = await _packageRepository.GetByIdAsync(dto.BenefitPackageId, cancellationToken);
        if (package == null)
        {
            throw new InvalidOperationException($"Benefit package with ID {dto.BenefitPackageId} not found.");
        }

        // Update - FOLOSEȘTE NUMELE EXACT din entity! ← FIX
        plan.UpdatePrice(dto.Price);
        plan.ChangeBenefitPackage(dto.BenefitPackageId);  
        plan.SetRecurring(dto.IsRecurring);  
        plan.UpdateInstallmentSettings(dto.AllowInstallments, dto.MaxInstallments);  

        if (dto.IsActive)
            plan.Activate();
        else
            plan.Deactivate();

        await _planRepository.UpdateAsync(plan, cancellationToken);
        return MapToDto(plan);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var plan = await _planRepository.GetByIdAsync(id, cancellationToken);
        if (plan == null)
        {
            throw new KeyNotFoundException($"Subscription plan with ID {id} not found.");
        }

        await _planRepository.DeleteAsync(plan, cancellationToken);
    }

    // Private mapping helper
    private static SubscriptionPlanDto MapToDto(SubscriptionPlan plan)
    {
        return new SubscriptionPlanDto
        {
            Id = plan.Id,
            Type = plan.Type.ToString(),
            DurationInMonths = plan.DurationInMonths,
            Price = plan.Price,
            BenefitPackageId = plan.BenefitPackageId,
            BenefitPackageName = plan.BenefitPackage?.Name ?? string.Empty,
            IsRecurring = plan.IsRecurring,
            AllowInstallments = plan.AllowInstallments,
            MaxInstallments = plan.MaxInstallments,
            IsActive = plan.IsActive,
            BenefitPackage = plan.BenefitPackage != null ? MapBenefitPackageToDto(plan.BenefitPackage) : null,
            CreatedAt = plan.CreatedAt,
            UpdatedAt = plan.UpdatedAt
        };
    }

    private static BenefitPackageDto MapBenefitPackageToDto(BenefitPackage package)
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