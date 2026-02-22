using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;

public class PremiumBenefitsPackage : IBenefitsPackage
{
    public string GetRecommendedTier() => "Premium";
    public bool IsPremiumTier() => true;

}
