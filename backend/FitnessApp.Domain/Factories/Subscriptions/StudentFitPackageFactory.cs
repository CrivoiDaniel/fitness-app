using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class StudentFitPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new StudentFitSubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new StudentFitPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new StudentBenefitsPackage();

}
