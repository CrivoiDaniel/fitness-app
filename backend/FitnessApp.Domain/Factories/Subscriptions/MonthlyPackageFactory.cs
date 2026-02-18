using System;
using FitnessApp.Domain.Factories.Subscriptions.Products;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class MonthlyPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new MonthlySubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new MonthlyPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new StandardBenefitsPackage();
}
