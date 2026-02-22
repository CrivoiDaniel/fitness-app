using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.DTOs.Statistics;      // ← DTOs (pentru return types)
using FitnessApp.Application.Features.Statistics;  // ← Service Interface (pentru IStatisticsService)

namespace FitnessApp.API.Controllers;

/// <summary>
/// Statistics API Controller
/// Provides subscription and revenue analytics
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    
    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    /// <summary>
    /// Get comprehensive subscription statistics
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SubscriptionStatisticsDto), 200)]
    public async Task<ActionResult<SubscriptionStatisticsDto>> GetStatistics(CancellationToken cancellationToken)
    {
        var statistics = await _statisticsService.GetStatisticsAsync(cancellationToken);
        return Ok(statistics);
    }
    
    /// <summary>
    /// Get revenue breakdown by time period
    /// </summary>
    [HttpGet("revenue")]
    [ProducesResponseType(typeof(RevenueBreakdownDto), 200)]
    public async Task<ActionResult<RevenueBreakdownDto>> GetRevenueBreakdown(CancellationToken cancellationToken)
    {
        var revenue = await _statisticsService.GetRevenueBreakdownAsync(cancellationToken);
        return Ok(revenue);
    }
    
    /// <summary>
    /// Get subscription trends (last 12 months)
    /// </summary>
    [HttpGet("trends")]
    [ProducesResponseType(typeof(List<TrendDataPointDto>), 200)]
    public async Task<ActionResult<List<TrendDataPointDto>>> GetSubscriptionTrends(CancellationToken cancellationToken)
    {
        var trends = await _statisticsService.GetSubscriptionTrendsAsync(cancellationToken);
        return Ok(trends);
    }
    
    /// <summary>
    /// Get subscriptions expiring in next N days
    /// </summary>
    [HttpGet("expiring")]
    [ProducesResponseType(typeof(List<ExpiringSubscriptionDto>), 200)]
    public async Task<ActionResult<List<ExpiringSubscriptionDto>>> GetExpiringSubscriptions(
        [FromQuery] int daysAhead = 30, 
        CancellationToken cancellationToken = default)
    {
        if (daysAhead < 1 || daysAhead > 365)
        {
            return BadRequest(new { message = "daysAhead must be between 1 and 365" });
        }
        
        var expiring = await _statisticsService.GetExpiringSubscriptionsAsync(daysAhead, cancellationToken);
        return Ok(expiring);
    }
    
    /// <summary>
    /// Get cache information
    /// </summary>
    [HttpGet("cache-info")]
    [ProducesResponseType(typeof(CacheInfoDto), 200)]
    public async Task<ActionResult<CacheInfoDto>> GetCacheInfo(CancellationToken cancellationToken)
    {
        var cacheInfo = await _statisticsService.GetCacheInfoAsync(cancellationToken);
        return Ok(cacheInfo);
    }
    
    /// <summary>
    /// Refresh statistics cache
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(200)]
    public async Task<ActionResult> RefreshCache(CancellationToken cancellationToken)
    {
        await _statisticsService.RefreshCacheAsync(cancellationToken);
        return Ok(new { message = "Cache refreshed successfully", timestamp = DateTime.UtcNow });
    }
    
    /// <summary>
    /// Get dashboard summary
    /// </summary>
    [HttpGet("dashboard")]
    [ProducesResponseType(200)]
    public async Task<ActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        var stats = await _statisticsService.GetStatisticsAsync(cancellationToken);
        var revenue = await _statisticsService.GetRevenueBreakdownAsync(cancellationToken);
        
        return Ok(new
        {
            totalSubscriptions = stats.TotalSubscriptions,
            activeSubscriptions = stats.ActiveSubscriptions,
            totalRevenue = stats.TotalRevenue,
            monthlyRevenue = stats.MonthlyRevenue,
            growthRate = stats.GrowthRate,
            churnRate = stats.ChurnRate,
            revenueBreakdown = revenue,
            calculatedAt = stats.CalculatedAt
        });
    }
}