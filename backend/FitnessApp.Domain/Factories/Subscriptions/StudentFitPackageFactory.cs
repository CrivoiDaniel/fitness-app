using System;
using FitnessApp.Domain.Factories.Subscriptions.Products;

namespace FitnessApp.Domain.Factories.Subscriptions;

public class StudentFitPackageFactory : ISubscriptionPackageFactory
{
    public ISubscriptionPlan CreatePlan() => new StudentFitSubscriptionPlan();
    public IPaymentPlan CreatePaymentPlan() => new StudentFitPaymentPlan();
    public IBenefitsPackage CreateBenefitsPackage() => new StudentBenefitsPackage();

}
