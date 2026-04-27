using FitnessApp.Domain.Strategy;

namespace FitnessApp.Application.Strategy;

/// <summary>
/// The Context maintains a reference to one of the strategy objects.
/// The Context doesn't know the concrete class of a strategy.
/// It works with all strategies via the strategy interface.
/// 
/// Definiție: Strategy este un pattern de design comportamental care îți permite să definești o
/// familie de algoritmi (în cazul tău, tipuri de reduceri), să îi pui în clase separate și să îi faci interschimbabili la runtime.
/// 
/// Problema rezolvată în codul tău: Înainte, calculul prețului final în funcție de diverse cupoane sau reduceri ar fi
/// necesitat un bloc uriaș de cod de tip if/else sau switch în clasa de checkout. Orice reducere nouă (ex: "Reducere de Paște") 
/// ar fi însemnat modificarea codului de bază, riscând să strici funcționalitățile existente.
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
