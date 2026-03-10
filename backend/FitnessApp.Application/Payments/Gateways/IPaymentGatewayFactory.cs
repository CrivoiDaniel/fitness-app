using System;

namespace FitnessApp.Application.Payments.Gateways;

public interface IPaymentGatewayFactory
{
    IPaymentGateway GetGateway();
    IPaymentGateway GetGateway(string provider); // NEW
    string DefaultCurrency { get; }
}