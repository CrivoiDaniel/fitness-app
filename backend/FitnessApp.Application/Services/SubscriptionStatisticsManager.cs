using System;
using FitnessApp.Application.Services.Statistics;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Domain.Interfaces.Statistics;

namespace FitnessApp.Application.Services;

/// <summary>
/// Singleton Pattern: Subscription Statistics Manager
/// Thread-safe using double-check locking pattern
/// SOLID Principles:
/// - Single Responsibility: Orchestrates statistics calculations
/// - Open/Closed: Extensible through interfaces
/// - Dependency Inversion: Depends on abstractions (interfaces)
/// </summary>
public sealed class SubscriptionStatisticsManager
{
    // ========== SINGLETON IMPLEMENTATION ==========
    
    private static SubscriptionStatisticsManager? _instance;
    private static readonly object _lock = new object();
    
    /// <summary>
    /// Thread-safe Singleton instance
    /// Double-check locking pattern for performance
    /// </summary>
    public static SubscriptionStatisticsManager Instance
    {
        get
        {
            // First check (no locking)
            if (_instance == null)
            {
                // Lock for thread safety
                lock (_lock)
                {
                    // Second check (with locking)
                    if (_instance == null)
                    {
                        _instance = new SubscriptionStatisticsManager();
                    }
                }
            }
            return _instance;
        }
    }
    
    // ========== DEPENDENCIES (Abstractions - SOLID DIP) ==========
    
    private readonly IStatisticsCache _cache;
    private readonly IRevenueCalculator _revenueCalculator;
    private readonly ITrendCalculator _trendCalculator;
    private readonly IMetricsCalculator _metricsCalculator;
    
    // ========== PRIVATE CONSTRUCTOR ==========
    
    /// <summary>
    /// Private constructor prevents direct instantiation
    /// Only accessible through Instance property
    /// </summary>
    private SubscriptionStatisticsManager()
    {
        // Initialize dependencies
        _cache = new StatisticsCache(TimeSpan.FromMinutes(5));
        _revenueCalculator = new RevenueCalculator();
        _trendCalculator = new TrendCalculator();
        _metricsCalculator = new MetricsCalculator();
        
        Console.WriteLine("[SubscriptionStatisticsManager] Singleton instance created at {0}", DateTime.UtcNow);
    }
    
    // ========== PUBLIC API ==========
    
    /// <summary>
    /// Update cache with fresh data from database
    /// </summary>
    public void UpdateCache(IEnumerable<Subscription> subscriptions, IEnumerable<Payment> payments)
    {
        _cache.UpdateCache(subscriptions, payments);
        Console.WriteLine("[SubscriptionStatisticsManager] Cache updated with {0} subscriptions and {1} payments", 
            subscriptions.Count(), payments.Count());
    }
    
    /// <summary>
    /// Check if cache has expired
    /// </summary>
    public bool IsCacheExpired()
    {
        return _cache.IsExpired();
    }
    
    /// <summary>
    /// Clear all cached data
    /// </summary>
    public void ClearCache()
    {
        _cache.Clear();
        Console.WriteLine("[SubscriptionStatisticsManager] Cache cleared");
    }
    
    /// <summary>
    /// Get comprehensive subscription statistics
    /// </summary>
    public StatisticsResult GetStatistics()
    {
        var subscriptions = _cache.GetCachedSubscriptions().ToList();
        var payments = _cache.GetCachedPayments().ToList();
        
        var total = subscriptions.Count;
        var active = _metricsCalculator.CountByStatus(subscriptions, SubscriptionStatus.Active);
        var pending = _metricsCalculator.CountByStatus(subscriptions, SubscriptionStatus.Pending);
        var expired = _metricsCalculator.CountByStatus(subscriptions, SubscriptionStatus.Expired);
        var cancelled = _metricsCalculator.CountByStatus(subscriptions, SubscriptionStatus.Cancelled);
        
        var successfulPayments = payments.Count(p => p.Status == PaymentStatus.Success);
        var failedPayments = payments.Count(p => p.Status == PaymentStatus.Failed);
        
        return new StatisticsResult
        {
            CalculatedAt = DateTime.UtcNow,
            
            // Subscription metrics
            TotalSubscriptions = total,
            ActiveSubscriptions = active,
            PendingSubscriptions = pending,
            ExpiredSubscriptions = expired,
            CancelledSubscriptions = cancelled,
            
            // Percentages
            ActivePercentage = _metricsCalculator.CalculatePercentage(active, total),
            PendingPercentage = _metricsCalculator.CalculatePercentage(pending, total),
            ExpiredPercentage = _metricsCalculator.CalculatePercentage(expired, total),
            CancelledPercentage = _metricsCalculator.CalculatePercentage(cancelled, total),
            
            // Revenue metrics
            TotalRevenue = _revenueCalculator.CalculateTotalRevenue(payments),
            MonthlyRevenue = _revenueCalculator.CalculateMonthlyRevenue(payments),
            YearlyRevenue = _revenueCalculator.CalculateYearlyRevenue(payments),
            
            // Payment metrics
            TotalPayments = payments.Count,
            SuccessfulPayments = successfulPayments,
            FailedPayments = failedPayments,
            
            // Growth metrics
            GrowthRate = _trendCalculator.CalculateGrowthRate(subscriptions),
            ChurnRate = _trendCalculator.CalculateChurnRate(subscriptions),
            
            // Breakdown
            SubscriptionsByType = _metricsCalculator.GroupByType(subscriptions)
        };
    }
    
    /// <summary>
    /// Get revenue breakdown by period
    /// </summary>
    public RevenueResult GetRevenueBreakdown()
    {
        var payments = _cache.GetCachedPayments();
        
        return new RevenueResult
        {
            Today = _revenueCalculator.CalculateDailyRevenue(payments),
            ThisWeek = _revenueCalculator.CalculateWeeklyRevenue(payments),
            ThisMonth = _revenueCalculator.CalculateMonthlyRevenue(payments),
            ThisYear = _revenueCalculator.CalculateYearlyRevenue(payments),
            AllTime = _revenueCalculator.CalculateTotalRevenue(payments)
        };
    }
    
    /// <summary>
    /// Get subscription trends (last 12 months)
    /// </summary>
    public List<TrendResult> GetSubscriptionTrends()
    {
        var subscriptions = _cache.GetCachedSubscriptions().ToList();
        var trends = new List<TrendResult>();
        var now = DateTime.UtcNow;
        
        for (int i = 11; i >= 0; i--)
        {
            var month = now.AddMonths(-i);
            var startOfMonth = new DateTime(month.Year, month.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);
            
            var created = subscriptions.Count(s => s.CreatedAt >= startOfMonth && s.CreatedAt < endOfMonth);
            var cancelled = subscriptions.Count(s => 
                s.Status == SubscriptionStatus.Cancelled && 
                s.UpdatedAt >= startOfMonth && 
                s.UpdatedAt < endOfMonth);
            
            trends.Add(new TrendResult
            {
                Period = startOfMonth.ToString("MMM yyyy"),
                NewSubscriptions = created,
                CancelledSubscriptions = cancelled,
                NetGrowth = created - cancelled
            });
        }
        
        return trends;
    }
    
    /// <summary>
    /// Get subscriptions expiring in next N days
    /// </summary>
    public List<ExpiringResult> GetExpiringSubscriptions(int daysAhead = 30)
    {
        var now = DateTime.UtcNow;
        var cutoffDate = now.AddDays(daysAhead);
        
        return _cache.GetCachedSubscriptions()
            .Where(s => 
                s.Status == SubscriptionStatus.Active &&
                s.EndDate.HasValue &&
                s.EndDate.Value <= cutoffDate &&
                s.EndDate.Value >= now)
            .OrderBy(s => s.EndDate)
            .Select(s => new ExpiringResult
            {
                SubscriptionId = s.Id,
                ClientId = s.ClientId,
                ClientName = s.Client?.User?.GetFullName() ?? "Unknown",
                PlanType = s.SubscriptionPlan?.Type.ToString() ?? "Unknown",
                EndDate = s.EndDate!.Value,
                DaysRemaining = (s.EndDate!.Value - now).Days
            })
            .ToList();
    }
    
    /// <summary>
    /// Get cache information
    /// </summary>
    public CacheResult GetCacheInfo()
    {
        var subscriptions = _cache.GetCachedSubscriptions().ToList();
        var payments = _cache.GetCachedPayments().ToList();
        
        return new CacheResult
        {
            IsExpired = _cache.IsExpired(),
            CachedSubscriptions = subscriptions.Count,
            CachedPayments = payments.Count
        };
    }
}

// ========== RESULT CLASSES (Internal to Domain) ==========

/// <summary>
/// Statistics calculation result
/// </summary>
public class StatisticsResult
{
    public DateTime CalculatedAt { get; set; }
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int PendingSubscriptions { get; set; }
    public int ExpiredSubscriptions { get; set; }
    public int CancelledSubscriptions { get; set; }
    public decimal ActivePercentage { get; set; }
    public decimal PendingPercentage { get; set; }
    public decimal ExpiredPercentage { get; set; }
    public decimal CancelledPercentage { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }
    public int TotalPayments { get; set; }
    public int SuccessfulPayments { get; set; }
    public int FailedPayments { get; set; }
    public decimal GrowthRate { get; set; }
    public decimal ChurnRate { get; set; }
    public Dictionary<string, int> SubscriptionsByType { get; set; } = new();
}

/// <summary>
/// Revenue breakdown result
/// </summary>
public class RevenueResult
{
    public decimal Today { get; set; }
    public decimal ThisWeek { get; set; }
    public decimal ThisMonth { get; set; }
    public decimal ThisYear { get; set; }
    public decimal AllTime { get; set; }
}

/// <summary>
/// Trend data point result
/// </summary>
public class TrendResult
{
    public string Period { get; set; } = string.Empty;
    public int NewSubscriptions { get; set; }
    public int CancelledSubscriptions { get; set; }
    public int NetGrowth { get; set; }
}

/// <summary>
/// Expiring subscription result
/// </summary>
public class ExpiringResult
{
    public int SubscriptionId { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string PlanType { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public int DaysRemaining { get; set; }
}

/// <summary>
/// Cache information result
/// </summary>
public class CacheResult
{
    public bool IsExpired { get; set; }
    public int CachedSubscriptions { get; set; }
    public int CachedPayments { get; set; }
}