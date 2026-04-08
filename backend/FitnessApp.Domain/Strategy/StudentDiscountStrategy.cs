using System;

namespace FitnessApp.Domain.Strategy;

/// <summary>
/// Concrete Strategy for Student Discount.
/// Applies a 20% discount.
/// </summary>
public class StudentDiscountStrategy : IDiscountStrategy
{
    public string Name => "Student Discount (20%)";

    public decimal ApplyDiscount(decimal price)
    {
        return Math.Round(price * 0.8m, 2);
    }
}
