namespace FitnessApp.Domain.Strategy;

/// <summary>
/// Concrete Strategy for No Discount.
/// Returns the original price.
/// </summary>
public class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "Standard Price";

    public decimal ApplyDiscount(decimal price)
    {
        return price;
    }
}
