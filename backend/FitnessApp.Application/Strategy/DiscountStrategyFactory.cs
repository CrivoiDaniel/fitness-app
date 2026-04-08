using FitnessApp.Domain.Strategy;

namespace FitnessApp.Application.Strategy;

/// <summary>
/// A factory to create the appropriate strategy based on a string name.
/// This simplifies client code and helps in switching strategies dynamically.
/// </summary>
public static class DiscountStrategyFactory
{
    public static IDiscountStrategy CreateStrategy(string strategyName)
    {
        return strategyName?.ToLower() switch
        {
            "student" => new StudentDiscountStrategy(),
            "seasonal" => new SeasonalDiscountStrategy(),
            _ => new NoDiscountStrategy()
        };
    }
}
