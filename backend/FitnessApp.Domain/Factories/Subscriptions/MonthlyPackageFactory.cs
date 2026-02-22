using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class MonthlyPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new MonthlySubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new MonthlyPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new StandardBenefitsPackage();
}
