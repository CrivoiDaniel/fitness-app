using FitnessApp.Domain.Decorator;

namespace FitnessApp.Infrastructure.Repositories.Decorator;


public interface IPaymentGatewayLogRepository
{
    Task AddAsync(PaymentGatewayLog log, CancellationToken cancellationToken = default);
    Task<List<PaymentGatewayLog>> GetLatestAsync(int take = 100, CancellationToken cancellationToken = default);
}