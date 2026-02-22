using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class QuarterlyPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new QuarterlySubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new QuarterlyPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new PlusBenefitsPackage();
}
