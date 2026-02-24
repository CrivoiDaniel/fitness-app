using System;

namespace FitnessApp.Application.DTOs.Statistics;

public class SubscriptionStatisticsDto
{
    // Metadata
    public DateTime CalculatedAt { get; set; }
    public DateTime CacheExpiresAt { get; set; }
    
    // Subscription counts
    public int TotalSubscriptions { get; set; }
    public int ActiveSubscriptions { get; set; }
    public int PendingSubscriptions { get; set; }
    public int ExpiredSubscriptions { get; set; }
    public int CancelledSubscriptions { get; set; }
    
    // Percentages
    public decimal ActivePercentage { get; set; }
    public decimal PendingPercentage { get; set; }
    public decimal ExpiredPercentage { get; set; }
    public decimal CancelledPercentage { get; set; }
    
    // Revenue
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal YearlyRevenue { get; set; }
    public decimal AverageSubscriptionValue { get; set; }
    
    // Payments
    public int TotalPayments { get; set; }
    public int SuccessfulPayments { get; set; }
    public int FailedPayments { get; set; }
    public int PendingPayments { get; set; }
    public decimal PaymentSuccessRate { get; set; }
    
    // Metrics
    public decimal GrowthRate { get; set; }
    public decimal ChurnRate { get; set; }
    
    // Breakdown
    public Dictionary<string, int> SubscriptionsByType { get; set; } = new();
}