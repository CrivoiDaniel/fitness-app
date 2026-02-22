using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;

public class StudentBenefitsPackage : IBenefitsPackage
{
    public string GetRecommendedTier() => "Student";
    public bool IsPremiumTier() => false;

}
