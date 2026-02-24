using System;
using FitnessApp.Application.DTOs.Statistics;

namespace FitnessApp.Application.Features.Statistics;


/// <summary>
/// Statistics service interface
/// </summary>
public interface IStatisticsService
{
    Task<SubscriptionStatisticsDto> GetStatisticsAsync(CancellationToken cancellationToken = default);
    Task<RevenueBreakdownDto> GetRevenueBreakdownAsync(CancellationToken cancellationToken = default);
    Task<List<TrendDataPointDto>> GetSubscriptionTrendsAsync(CancellationToken cancellationToken = default);
    Task<List<ExpiringSubscriptionDto>> GetExpiringSubscriptionsAsync(int daysAhead = 30, CancellationToken cancellationToken = default);
    Task<CacheInfoDto> GetCacheInfoAsync(CancellationToken cancellationToken = default);
    Task RefreshCacheAsync(CancellationToken cancellationToken = default);
}