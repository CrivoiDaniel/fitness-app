using System;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Services.Subscriptions;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public PaymentService(
        IPaymentRepository paymentRepository,
        ISubscriptionRepository subscriptionRepository)
    {
        _paymentRepository = paymentRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetAllAsync(cancellationToken);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetBySubscriptionIdAsync(int subscriptionId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetBySubscriptionIdAsync(subscriptionId, cancellationToken);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetByClientIdAsync(clientId, cancellationToken);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetByStatusAsync(status, cancellationToken);
        return payments.Select(MapToDto);
    }

    public async Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default)
    {
        var payments = await _paymentRepository.GetPendingPaymentsAsync(cancellationToken);
        return payments.Select(MapToDto);
    }

    public async Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        return payment == null ? null : MapToDto(payment);
    }

    public async Task<PaymentDto?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByTransactionIdAsync(transactionId, cancellationToken);
        return payment == null ? null : MapToDto(payment);
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        // Verifică că subscription există
        var subscription = await _subscriptionRepository.GetByIdAsync(dto.SubscriptionId, cancellationToken);
        if (subscription == null)
        {
            throw new InvalidOperationException($"Subscription with ID {dto.SubscriptionId} not found.");
        }

        // Creează payment
        var payment = new Payment(
            dto.SubscriptionId,
            dto.Amount,
            dto.PaymentDate,
            dto.InstallmentNumber,
            dto.TransactionId);

        var created = await _paymentRepository.AddAsync(payment, cancellationToken);
        return MapToDto(created);
    }

    public async Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto dto, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found.");
        }

        // Update status - DOAR metodele disponibile ← FIX
        switch (dto.Status)
        {
            case PaymentStatus.Success:
                payment.MarkAsSuccess();
                break;
            case PaymentStatus.Failed:
                payment.MarkAsFailed();
                break;
            case PaymentStatus.Pending:
                // Pending e status default, nu facem nimic
                break;
        }

        if (!string.IsNullOrEmpty(dto.TransactionId))
        {
            payment.UpdateTransactionId(dto.TransactionId);
        }

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return MapToDto(payment);
    }

    public async Task<PaymentDto> MarkAsSuccessAsync(int id, string? transactionId = null, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found.");
        }

        payment.MarkAsSuccess(transactionId);  // ← Entity primește transactionId direct

        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        return MapToDto(payment);
    }

    public async Task<PaymentDto> MarkAsFailedAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found.");
        }

        payment.MarkAsFailed();
        await _paymentRepository.UpdateAsync(payment, cancellationToken);
        
        return MapToDto(payment);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.GetByIdAsync(id, cancellationToken);
        if (payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {id} not found.");
        }

        await _paymentRepository.DeleteAsync(payment, cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await _paymentRepository.GetTotalRevenueAsync(startDate, endDate, cancellationToken);
    }

    // Private mapping helper
    private static PaymentDto MapToDto(Payment payment)
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