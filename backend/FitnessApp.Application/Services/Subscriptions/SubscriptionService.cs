
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.DTOs.Subscriptions.Subscription;
using FitnessApp.Application.Interfaces.Subscriptions;

namespace FitnessApp.Application.Features.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly IClientRepository _clientRepository;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ISubscriptionPlanRepository planRepository,
        IClientRepository clientRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _planRepository = planRepository;
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<SubscriptionDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.GetAllAsync(cancellationToken);
        return subscriptions.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.GetByClientIdAsync(clientId, cancellationToken);
        return subscriptions.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetActiveByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.GetActiveByClientIdAsync(clientId, cancellationToken);
        return subscriptions.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetByStatusAsync(SubscriptionStatus status, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.GetByStatusAsync(status, cancellationToken);
        return subscriptions.Select(MapToDto);
    }

    public async Task<IEnumerable<SubscriptionDto>> GetExpiringAsync(int withinDays, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionRepository.GetExpiringAsync(withinDays, cancellationToken);
        return subscriptions.Select(MapToDto);
    }

    public async Task<SubscriptionDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        return subscription == null ? null : MapToDto(subscription);
    }

    public async Task<SubscriptionDto?> GetWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetWithDetailsAsync(id, cancellationToken);
        return subscription == null ? null : MapToDto(subscription);
    }

    public async Task<SubscriptionDto> CreateAsync(CreateSubscriptionDto dto, CancellationToken cancellationToken = default)
    {
        // Verifică că client există - FIX: fără cancellationToken
        var client = await _clientRepository.GetByIdAsync(dto.ClientId);
        if (client == null)
        {
            throw new InvalidOperationException($"Client with ID {dto.ClientId} not found.");
        }

        // Verifică că plan există și e activ
        var plan = await _planRepository.GetByIdAsync(dto.SubscriptionPlanId, cancellationToken);
        if (plan == null)
        {
            throw new InvalidOperationException($"Subscription plan with ID {dto.SubscriptionPlanId} not found.");
        }

        if (!plan.IsActive)
        {
            throw new InvalidOperationException($"Subscription plan with ID {dto.SubscriptionPlanId} is not active.");
        }

        // Verifică dacă clientul are deja subscription activ pentru acest plan
        var hasActive = await _subscriptionRepository.HasActiveSubscriptionAsync(dto.ClientId, dto.SubscriptionPlanId, cancellationToken);
        if (hasActive)
        {
            throw new InvalidOperationException($"Client already has an active subscription for this plan.");
        }

        // Calculează end date
        DateTime? endDate = null;
        if (plan.DurationInMonths > 0)
        {
            endDate = dto.StartDate.AddMonths(plan.DurationInMonths);
        }

        // Creează subscription
        var subscription = new Subscription(
            dto.ClientId,
            dto.SubscriptionPlanId,
            dto.StartDate,
            endDate,
            dto.AutoRenew);

        var created = await _subscriptionRepository.AddAsync(subscription, cancellationToken);
        return MapToDto(created);
    }

    public async Task<SubscriptionDto> UpdateAsync(int id, UpdateSubscriptionDto dto, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }

        // Update folosind metodele CORECTE din entity
        switch (dto.Status)
        {
            case SubscriptionStatus.Active:
                subscription.Activate();
                break;
            case SubscriptionStatus.Cancelled:
                subscription.Cancel();
                break;
            case SubscriptionStatus.Expired:
                subscription.MarkAsExpired();
                break;
        }

        subscription.SetAutoRenew(dto.AutoRenew);

        if (dto.EndDate.HasValue && dto.EndDate.Value != subscription.EndDate)
        {
            try
            {
                subscription.Renew(dto.EndDate.Value);
            }
            catch
            {
                // Ignore dacă new date nu e valid
            }
        }

        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        return MapToDto(subscription);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }

        await _subscriptionRepository.DeleteAsync(subscription, cancellationToken);
    }

    public async Task<SubscriptionDto> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }

        subscription.Cancel();
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        
        return MapToDto(subscription);
    }

    public async Task<SubscriptionDto> RenewAsync(int id, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionRepository.GetWithDetailsAsync(id, cancellationToken);
        if (subscription == null)
        {
            throw new KeyNotFoundException($"Subscription with ID {id} not found.");
        }

        if (subscription.Status != SubscriptionStatus.Expired && subscription.Status != SubscriptionStatus.Active)
        {
            throw new InvalidOperationException("Only expired or active subscriptions can be renewed.");
        }

        var plan = subscription.SubscriptionPlan;
        if (plan == null || !plan.IsActive)
        {
            throw new InvalidOperationException("Cannot renew: subscription plan is not active.");
        }

        var currentEndDate = subscription.EndDate ?? subscription.StartDate;
        var newEndDate = currentEndDate.AddMonths(plan.DurationInMonths);
        
        subscription.Renew(newEndDate);
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);

        return MapToDto(subscription);
    }

    // Private mapping helpers
    private static SubscriptionDto MapToDto(Subscription subscription)
    {
        return new SubscriptionDto
        {
            Id = subscription.Id,
            ClientId = subscription.ClientId,
            ClientName = subscription.Client?.User != null 
                ? $"{subscription.Client.User.FirstName} {subscription.Client.User.LastName}".Trim() 
                : string.Empty,  // ← FIX
            SubscriptionPlanId = subscription.SubscriptionPlanId,
            PlanType = subscription.SubscriptionPlan?.Type.ToString() ?? string.Empty,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Status = subscription.Status.ToString(),
            AutoRenew = subscription.AutoRenew,
            SubscriptionPlan = subscription.SubscriptionPlan != null ? MapPlanToDto(subscription.SubscriptionPlan) : null,
            Payments = subscription.Payments?.Select(MapPaymentToDto).ToList() ?? new List<PaymentDto>(),
            CreatedAt = subscription.CreatedAt,
            UpdatedAt = subscription.UpdatedAt
        };
    }

    private static SubscriptionPlanDto MapPlanToDto(SubscriptionPlan plan)
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
            CreatedAt = plan.CreatedAt,
            UpdatedAt = plan.UpdatedAt
        };
    }

    private static PaymentDto MapPaymentToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            SubscriptionId = payment.SubscriptionId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            Status = payment.Status.ToString(),
            InstallmentNumber = payment.InstallmentNumber,
            TransactionId = payment.TransactionId,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
}