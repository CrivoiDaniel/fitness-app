using System;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Subscriptions;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> GetBySubscriptionIdAsync(
        int subscriptionId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.SubscriptionId == subscriptionId)
            .OrderBy(p => p.InstallmentNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByStatusAsync(
        PaymentStatus status, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Status == status)
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Client)
                    .ThenInclude(c => c.User)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payment?> GetByTransactionIdAsync(
        string transactionId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Client)
                    .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.TransactionId == transactionId, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetByClientIdAsync(
        int clientId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Subscription)
                .ThenInclude(s => s.SubscriptionPlan)
            .Where(p => p.Subscription.ClientId == clientId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => p.Status == PaymentStatus.Pending)
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Client)
                    .ThenInclude(c => c.User)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetSuccessfulPaymentsByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => 
                p.Status == PaymentStatus.Success &&
                p.PaymentDate >= startDate &&
                p.PaymentDate <= endDate)
            .Include(p => p.Subscription)
                .ThenInclude(s => s.Client)
                    .ThenInclude(c => c.User)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<Payment> query = _dbSet.Where(p => p.Status == PaymentStatus.Success);

        if (startDate.HasValue)
            query = query.Where(p => p.PaymentDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(p => p.PaymentDate <= endDate.Value);

        return await query.SumAsync(p => p.Amount, cancellationToken);
    }
}