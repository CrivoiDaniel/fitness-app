using System;
using FitnessApp.Domain.Factories.Subscriptions.Products;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class QuarterlyPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new QuarterlySubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new QuarterlyPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new PlusBenefitsPackage();
}
