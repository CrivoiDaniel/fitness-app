using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;
namespace FitnessApp.Domain.Factories.Subscriptions;

public class YearlyPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new YearlySubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new YearlyPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new PremiumBenefitsPackage();

}
