using System;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetBySubscriptionIdAsync(int subscriptionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaymentDto?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto, CancellationToken cancellationToken = default);
    Task<PaymentDto> UpdateAsync(int id, UpdatePaymentDto dto, CancellationToken cancellationToken = default);
    Task<PaymentDto> MarkAsSuccessAsync(int id, string? transactionId = null, CancellationToken cancellationToken = default);
    Task<PaymentDto> MarkAsFailedAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}