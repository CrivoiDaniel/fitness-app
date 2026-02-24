using System;
using FitnessApp.Domain.Entities.Subscriptions;

namespace FitnessApp.Domain.Interfaces.Statistics;

/// <summary>
/// Revenue calculation interface
/// </summary>
public interface IRevenueCalculator
{
    decimal CalculateTotalRevenue(IEnumerable<Payment> payments);
    decimal CalculateMonthlyRevenue(IEnumerable<Payment> payments);
    decimal CalculateYearlyRevenue(IEnumerable<Payment> payments);
    decimal CalculateDailyRevenue(IEnumerable<Payment> payments);
    decimal CalculateWeeklyRevenue(IEnumerable<Payment> payments);
}