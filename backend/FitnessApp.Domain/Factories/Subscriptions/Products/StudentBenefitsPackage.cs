using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class StudentBenefitsPackage : IBenefitsPackage
{
    public string GetRecommendedTier() => "Student";
    public bool IsPremiumTier() => false;

}
