using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Interfaces.Repositories.Subscriptions;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetBySubscriptionIdAsync(int subscriptionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
    Task<Payment?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetSuccessfulPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}
