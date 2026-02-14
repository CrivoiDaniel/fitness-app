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
}
