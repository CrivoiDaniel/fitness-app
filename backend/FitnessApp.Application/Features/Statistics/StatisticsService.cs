using System;
using FitnessApp.Application.DTOs.Statistics;
using FitnessApp.Application.Interfaces.Repositories.Subscriptions;
using FitnessApp.Application.Services;
namespace FitnessApp.Application.Features.Statistics;

/// <summary>
/// Statistics service implementation
/// Bridge between Domain (Singleton) and API
/// Maps Domain results to DTOs
/// </summary>
public class StatisticsService : IStatisticsService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPaymentRepository _paymentRepository;

    public StatisticsService(
        ISubscriptionRepository subscriptionRepository,
        IPaymentRepository paymentRepository)
    {
        _subscriptionRepository = subscriptionRepository;
        _paymentRepository = paymentRepository;
    }

    /// <summary>
    /// Get comprehensive statistics
    /// Auto-refreshes cache if expired
    /// </summary>
    public async Task<SubscriptionStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default)
    {
        // Check if cache is expired and refresh if needed
        if (SubscriptionStatisticsManager.Instance.IsCacheExpired())
        {
            await RefreshCacheAsync(cancellationToken);
        }

        // Get statistics from Domain Singleton
        var stats = SubscriptionStatisticsManager.Instance.GetStatistics();

        // Map Domain result to DTO
        return new SubscriptionStatisticsDto
        {
            CalculatedAt = stats.CalculatedAt,
            TotalSubscriptions = stats.TotalSubscriptions,
            ActiveSubscriptions = stats.ActiveSubscriptions,
            PendingSubscriptions = stats.PendingSubscriptions,
            ExpiredSubscriptions = stats.ExpiredSubscriptions,
            CancelledSubscriptions = stats.CancelledSubscriptions,
            ActivePercentage = stats.ActivePercentage,
            PendingPercentage = stats.PendingPercentage,
            ExpiredPercentage = stats.ExpiredPercentage,
            CancelledPercentage = stats.CancelledPercentage,
            TotalRevenue = stats.TotalRevenue,
            MonthlyRevenue = stats.MonthlyRevenue,
            YearlyRevenue = stats.YearlyRevenue,
            TotalPayments = stats.TotalPayments,
            SuccessfulPayments = stats.SuccessfulPayments,
            FailedPayments = stats.FailedPayments,
            GrowthRate = stats.GrowthRate,
            ChurnRate = stats.ChurnRate,
            SubscriptionsByType = stats.SubscriptionsByType
        };
    }

    /// <summary>
    /// Get revenue breakdown
    /// </summary>
    public async Task<RevenueBreakdownDto> GetRevenueBreakdownAsync(CancellationToken cancellationToken = default)
    {
        if (SubscriptionStatisticsManager.Instance.IsCacheExpired())
        {
            await RefreshCacheAsync(cancellationToken);
        }

        var revenue = SubscriptionStatisticsManager.Instance.GetRevenueBreakdown();

        return new RevenueBreakdownDto
        {
            Today = revenue.Today,
            ThisWeek = revenue.ThisWeek,
            ThisMonth = revenue.ThisMonth,
            ThisYear = revenue.ThisYear,
            AllTime = revenue.AllTime
        };
    }

    /// <summary>
    /// Get subscription trends
    /// </summary>
    public async Task<List<TrendDataPointDto>> GetSubscriptionTrendsAsync(CancellationToken cancellationToken = default)
    {
        if (SubscriptionStatisticsManager.Instance.IsCacheExpired())
        {
            await RefreshCacheAsync(cancellationToken);
        }

        var trends = SubscriptionStatisticsManager.Instance.GetSubscriptionTrends();

        return trends.Select(t => new TrendDataPointDto
        {
            Period = t.Period,
            NewSubscriptions = t.NewSubscriptions,
            CancelledSubscriptions = t.CancelledSubscriptions,
            NetGrowth = t.NetGrowth
        }).ToList();
    }

    /// <summary>
    /// Get expiring subscriptions
    /// </summary>
    public async Task<List<ExpiringSubscriptionDto>> GetExpiringSubscriptionsAsync(
        int daysAhead = 30,
        CancellationToken cancellationToken = default)
    {
        if (SubscriptionStatisticsManager.Instance.IsCacheExpired())
        {
            await RefreshCacheAsync(cancellationToken);
        }

        var expiring = SubscriptionStatisticsManager.Instance.GetExpiringSubscriptions(daysAhead);

        return expiring.Select(e => new ExpiringSubscriptionDto
        {
            SubscriptionId = e.SubscriptionId,
            ClientId = e.ClientId,
            ClientName = e.ClientName,
            PlanType = e.PlanType,
            EndDate = e.EndDate,
            DaysRemaining = e.DaysRemaining
        }).ToList();
    }

    /// <summary>
    /// Get cache information
    /// </summary>
    public async Task<CacheInfoDto> GetCacheInfoAsync(CancellationToken cancellationToken = default)
    {
        var cacheInfo = SubscriptionStatisticsManager.Instance.GetCacheInfo();

        return await Task.FromResult(new CacheInfoDto
        {
            LastUpdate = DateTime.UtcNow, // Simplified
            IsExpired = cacheInfo.IsExpired,
            CachedSubscriptions = cacheInfo.CachedSubscriptions,
            CachedPayments = cacheInfo.CachedPayments
        });
    }

    /// <summary>
    /// Refresh cache with fresh data from database
    /// </summary>
    public async Task RefreshCacheAsync(CancellationToken cancellationToken = default)
    {
        // Load fresh data from database
        var subscriptions = await _subscriptionRepository.GetAllWithDetailsAsync(cancellationToken);
        var payments = await _paymentRepository.GetAllAsync(cancellationToken);

        // Update Singleton cache
        SubscriptionStatisticsManager.Instance.UpdateCache(subscriptions, payments);
    }
}
