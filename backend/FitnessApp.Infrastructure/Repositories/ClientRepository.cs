using System;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Client?> GetByIdAsync(int id)
    {
        return  await _context.Clients
            .Include(c => c.user)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<Client?> GetByUserIdAsync(int userId)
    {
        return await _context.Clients
            .Include(c => c.user)
            .FirstOrDefaultAsync(c => c.user.Id == userId);
    }

    public async Task<List<Client>> GetAllAsync()
    {
        return await _context.Clients
            .Include(c => c.user)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Client>> GetActiveClientsAsync()
    {
        return await _context.Clients
            .Include(c => c.user)
            .Where(c => c.user.IsActive)
            .OrderByDescending( c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByUserIdAsync(int userId)
    {
        return await _context.Clients
            .AnyAsync(c => c.UserId == userId);
    }
    public async Task<Client> AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
        return client;
    }
    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Client client)
    {
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}
