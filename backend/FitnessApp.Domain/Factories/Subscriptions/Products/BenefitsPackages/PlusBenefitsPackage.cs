using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;

public class PlusBenefitsPackage : IBenefitsPackage
{
    public string GetRecommendedTier() => "Plus";
    public bool IsPremiumTier() => false;

}
