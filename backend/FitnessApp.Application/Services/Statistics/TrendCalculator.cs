using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Domain.Interfaces.Statistics;

namespace FitnessApp.Application.Services.Statistics;
public class TrendCalculator : ITrendCalculator
{
    public decimal CalculateGrowthRate(IEnumerable<Subscription> subscriptions)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        var previousMonth = startOfMonth.AddMonths(-1);
        
        var subsList = subscriptions.ToList();
        
        var previousCount = subsList.Count(s => 
            s.CreatedAt >= previousMonth && s.CreatedAt < startOfMonth);
        
        var currentCount = subsList.Count(s => 
            s.CreatedAt >= startOfMonth);
        
        if (previousCount == 0) return 0;
        
        return (decimal)(currentCount - previousCount) / previousCount * 100;
    }
    
    public decimal CalculateChurnRate(IEnumerable<Subscription> subscriptions)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        
        var subsList = subscriptions.ToList();
        
        var active = subsList.Count(s => s.Status == SubscriptionStatus.Active);
        var cancelledThisMonth = subsList.Count(s => 
            s.Status == SubscriptionStatus.Cancelled && 
            s.UpdatedAt >= startOfMonth);
        
        var totalBase = active + cancelledThisMonth;
        
        if (totalBase == 0) return 0;
        
        return (decimal)cancelledThisMonth / totalBase * 100;
    }
}