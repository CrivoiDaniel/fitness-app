using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Enums;
using FitnessApp.Domain.Interfaces.Statistics;

namespace FitnessApp.Application.Services.Statistics;

public class RevenueCalculator : IRevenueCalculator
{
    public decimal CalculateTotalRevenue(IEnumerable<Payment> payments)
    {
        return payments
            .Where(p => p.Status == PaymentStatus.Success)
            .Sum(p => p.Amount);
    }
    
    public decimal CalculateMonthlyRevenue(IEnumerable<Payment> payments)
    {
        var now = DateTime.UtcNow;
        return payments
            .Where(p => p.Status == PaymentStatus.Success 
                && p.PaymentDate.Year == now.Year 
                && p.PaymentDate.Month == now.Month)
            .Sum(p => p.Amount);
    }
    
    public decimal CalculateYearlyRevenue(IEnumerable<Payment> payments)
    {
        var now = DateTime.UtcNow;
        return payments
            .Where(p => p.Status == PaymentStatus.Success 
                && p.PaymentDate.Year == now.Year)
            .Sum(p => p.Amount);
    }
    
    public decimal CalculateDailyRevenue(IEnumerable<Payment> payments)
    {
        var today = DateTime.UtcNow.Date;
        return payments
            .Where(p => p.Status == PaymentStatus.Success 
                && p.PaymentDate.Date == today)
            .Sum(p => p.Amount);
    }
    
    public decimal CalculateWeeklyRevenue(IEnumerable<Payment> payments)
    {
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        return payments
            .Where(p => p.Status == PaymentStatus.Success 
                && p.PaymentDate >= weekAgo)
            .Sum(p => p.Amount);
    }
}