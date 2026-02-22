using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;

public class StandardBenefitsPackage : IBenefitsPackage
{
    public string GetRecommendedTier() => "Standard";
    public bool IsPremiumTier() => false;
}
