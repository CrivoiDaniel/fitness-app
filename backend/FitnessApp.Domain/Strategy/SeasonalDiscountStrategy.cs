using System;

namespace FitnessApp.Domain.Strategy;

/// <summary>
/// Concrete Strategy for Seasonal Discount.
/// Applies a 15% discount.
/// </summary>
public class SeasonalDiscountStrategy : IDiscountStrategy
{
    public string Name => "Seasonal Sale (15%)";

    public decimal ApplyDiscount(decimal price)
    {
        return Math.Round(price * 0.85m, 2);
    }
}
