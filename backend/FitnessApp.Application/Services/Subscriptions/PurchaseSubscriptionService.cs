using System;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.DTOs.Subscriptions.Purchase;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Application.Payments.Gateways;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Application.Services.Subscriptions;

public class PurchaseSubscriptionService : IPurchaseSubscriptionService
{
    private readonly IClientRepository _clientRepository;
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentGatewayFactory _gatewayFactory;

    public PurchaseSubscriptionService(
        IClientRepository clientRepository,
        ISubscriptionPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IPaymentService paymentService,
        IPaymentGatewayFactory gatewayFactory)
    {
        _clientRepository = clientRepository;
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _paymentService = paymentService;
        _gatewayFactory = gatewayFactory;
    }

    public async Task<PurchaseSubscriptionResultDto> PurchaseAsync(int userId, PurchaseSubscriptionRequestDto dto, CancellationToken cancellationToken = default)
    {
        // 1) Map UserId -> Client
        // IMPORTANT: aici depinde cum e modelat Client entity (UserId / Email)
        // In repo trebuie să existe o metodă de căutare (GetByUserIdAsync).
        // Dacă nu există, o adăugăm (vezi punctul 4).
        var client = await _clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException("Client profile not found for current user.");

        // 2) Plan
        var plan = await _planRepository.GetByIdAsync(dto.SubscriptionPlanId, cancellationToken);
        if (plan == null)
            throw new InvalidOperationException($"Subscription plan with ID {dto.SubscriptionPlanId} not found.");

        if (!plan.IsActive)
            throw new InvalidOperationException("Subscription plan is not active.");

        // 3) Create Subscription (direct in repo ca sa nu fie Admin-only controller dependent)
        DateTime startDate = DateTime.UtcNow.Date;
        DateTime? endDate = plan.DurationInMonths > 0 ? startDate.AddMonths(plan.DurationInMonths) : null;

        var subscription = new Subscription(
            clientId: client.Id,
            subscriptionPlanId: plan.Id,
            startDate: startDate,
            endDate: endDate,
            autoRenew: dto.AutoRenew
        );

        var createdSubscription = await _subscriptionRepository.AddAsync(subscription, cancellationToken);
        // IMPORTANT: setăm providerul ales în factory (din UI)
        _gatewayFactory.GetGateway(dto.Provider); // validare implicită

        var (paymentDto, charge) = await _paymentService.CreateWithGatewayAsync(new CreatePaymentDto
        {
            SubscriptionId = createdSubscription.Id,
            Amount = plan.Price,
            PaymentDate = DateTime.UtcNow,
            InstallmentNumber = 1,
            TransactionId = null
        }, cancellationToken);

        // 5) Return
        return new PurchaseSubscriptionResultDto
        {
            SubscriptionId = createdSubscription.Id,
            PaymentId = paymentDto.Id,
            PaymentStatus = paymentDto.Status,
            TransactionId = paymentDto.TransactionId,
            Provider = charge.Provider,           // "Stripe" / "PayPal(Demo)"
            ClientSecret = charge.ClientSecret    // IMPORTANT pentru Stripe Elements
        };
    }
}