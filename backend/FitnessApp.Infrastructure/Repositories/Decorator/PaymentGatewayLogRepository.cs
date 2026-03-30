using FitnessApp.Domain.Decorator;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Decorator;

public class PaymentGatewayLogRepository : IPaymentGatewayLogRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentGatewayLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PaymentGatewayLog log, CancellationToken cancellationToken = default)
    {
        _context.Set<PaymentGatewayLog>().Add(log);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<PaymentGatewayLog>> GetLatestAsync(int take = 100, CancellationToken cancellationToken = default)
    {
        take = Math.Clamp(take, 1, 500);

        return await _context.Set<PaymentGatewayLog>()
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}