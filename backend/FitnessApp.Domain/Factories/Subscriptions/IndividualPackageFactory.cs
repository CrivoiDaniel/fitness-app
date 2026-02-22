using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class IndividualPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new IndividualSubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new IndividualPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new StandardBenefitsPackage();

}
