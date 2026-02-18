using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

//Defines recommended benefit tier
public interface IBenefitsPackage
{
    string GetRecommendedTier();
    bool IsPremiumTier();
}
