using System;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Infrastructure.Data;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Users;

public class TrainerRepository : ITrainerRepository
{
    private readonly ApplicationDbContext _context;
    
    public TrainerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Trainer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
    
    public async Task<List<Trainer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
            .Include(t => t.User)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Trainer> AddAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        await _context.Trainers.AddAsync(trainer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return trainer;
    }
    
    public async Task UpdateAsync(Trainer trainer, CancellationToken cancellationToken = default)
    {
        _context.Trainers.Update(trainer);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var trainer = await GetByIdAsync(id, cancellationToken);
        if (trainer != null)
        {
            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
    
    // ✅ Implementează metodele lipsă
    
    public async Task<Trainer?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
    }
    
    public async Task<List<Trainer>> GetActiveTrainersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
            .Include(t => t.User)
            .Where(t => t.User != null && t.User.IsActive)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _context.Trainers
            .AnyAsync(t => t.UserId == userId, cancellationToken);
    }
}