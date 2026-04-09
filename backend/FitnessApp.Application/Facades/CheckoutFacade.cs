using System;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.DTOs.Subscriptions.Purchase;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Application.Observer;
using FitnessApp.Application.Payments.Gateways;
using FitnessApp.Application.Strategy;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Strategy;

namespace FitnessApp.Application.Facades;

/// <summary>
/// Facade for checkout workflow: purchase subscription + create payment through selected gateway.
/// Simplifies controller interaction with complex subsystem (repos + payment service + gateway factory).
/// </summary>
public class CheckoutFacade
{
    private readonly IClientRepository _clientRepository;
    private readonly ISubscriptionPlanRepository _planRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentGatewayFactory _gatewayFactory;
    private readonly ISubscriptionPublisher _publisher;

    public CheckoutFacade(
        IClientRepository clientRepository,
        ISubscriptionPlanRepository planRepository,
        ISubscriptionRepository subscriptionRepository,
        IPaymentService paymentService,
        IPaymentGatewayFactory gatewayFactory,
        ISubscriptionPublisher publisher)
    {
        _clientRepository = clientRepository;
        _planRepository = planRepository;
        _subscriptionRepository = subscriptionRepository;
        _paymentService = paymentService;
        _gatewayFactory = gatewayFactory;
        _publisher = publisher;
    }

    public async Task<PurchaseSubscriptionResultDto> PurchaseSubscriptionAsync(
        int userId,
        PurchaseSubscriptionRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException("Client profile not found for current user.");

        var plan = await _planRepository.GetByIdAsync(dto.SubscriptionPlanId, cancellationToken);
        if (plan == null)
            throw new InvalidOperationException($"Subscription plan with ID {dto.SubscriptionPlanId} not found.");

        if (!plan.IsActive)
            throw new InvalidOperationException("Subscription plan is not active.");

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

        // choose gateway (validates provider string)
        _gatewayFactory.GetGateway(dto.Provider);

        // STRATEGY PATTERN: Apply discount if requested
        decimal finalAmount = plan.Price;
        if (!string.IsNullOrWhiteSpace(dto.DiscountType))
        {
            var strategy = DiscountStrategyFactory.CreateStrategy(dto.DiscountType);
            var context = new DiscountStrategyContext(strategy);
            finalAmount = context.ExecuteStrategy(plan.Price);
        }

        var (paymentDto, charge) = await _paymentService.CreateWithGatewayAsync(new CreatePaymentDto
        {
            SubscriptionId = createdSubscription.Id,
            Amount = finalAmount,
            PaymentDate = DateTime.UtcNow,
            InstallmentNumber = 1,
            TransactionId = null
        }, dto.Provider, cancellationToken);
        
        // OBSERVER PATTERN: Notify observers (Email, Stats, etc.)
        // Adăugăm un log explicit pentru a fi demonstrat la laborator
        Console.WriteLine("\n[OBSERVER PATTERN] Step 1: Initiating notifications for purchase of plan {0} MDL", finalAmount);
        try 
        {
            await _publisher.NotifyPurchaseAsync(createdSubscription, finalAmount);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[OBSERVER ERROR] {0}", ex.Message);
        }

        return new PurchaseSubscriptionResultDto
        {
            SubscriptionId = createdSubscription.Id,
            PaymentId = paymentDto.Id,
            PaymentStatus = paymentDto.Status,
            TransactionId = paymentDto.TransactionId,
            Provider = charge.Provider,
            ClientSecret = charge.ClientSecret
        };
    }
}