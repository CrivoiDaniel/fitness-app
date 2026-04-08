using FitnessApp.Domain.Strategy;

namespace FitnessApp.Application.Strategy;

/// <summary>
/// The Context maintains a reference to one of the strategy objects.
/// The Context doesn't know the concrete class of a strategy.
/// It works with all strategies via the strategy interface.
/// </summary>
public class DiscountStrategyContext
{
    private IDiscountStrategy _strategy;

    // Usually the context accepts a strategy through the constructor,
    // and also provides a setter so that the strategy can be switched at runtime.
    public DiscountStrategyContext(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IDiscountStrategy strategy)
    {
        _strategy = strategy;
    }

    /// <summary>
    /// The Context delegates some work to the strategy object instead of
    /// implementing multiple versions of the algorithm on its own.
    /// </summary>
    public decimal ExecuteStrategy(decimal price)
    {
        return _strategy.ApplyDiscount(price);
    }
    
    public string GetStrategyName()
    {
        return _strategy.Name;
    }
}
