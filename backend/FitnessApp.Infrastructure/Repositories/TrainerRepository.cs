using System;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories;

public class TrainerRepository : ITrainerRepository
{
    private readonly ApplicationDbContext _context;
    public TrainerRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Trainer?> GetByIdAsync(int id)
    {
        return await _context.Trainers
            .Include(t => t.user)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<Trainer?> GetByUserIdAsync(int userId)
    {
        return await _context.Trainers
            .Include(t => t.user)
            .FirstOrDefaultAsync(t => t.user.Id == userId);
    }

    public async Task<List<Trainer>> GetAllAsync()
    {
        return await _context.Trainers
            .Include(c => c.user)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Trainer>> GetActiveTrainersAsync()
    {
        return await _context.Trainers
            .Include(c => c.user)
            .Where(c => c.user.IsActive)
            .OrderByDescending( c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByUserIdAsync(int userId)
    {
        return await _context.Trainers
            .AnyAsync(c => c.UserId == userId);
    }

    public async Task<Trainer> AddAsync(Trainer trainer)
    {
        await _context.Trainers.AddAsync(trainer);
        await _context.SaveChangesAsync();
        return trainer;
    }
    public async Task UpdateAsync(Trainer trainer)
    {
        _context.Trainers.Update(trainer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Trainer trainer)
    {
        _context.Trainers.Remove(trainer);
        await _context.SaveChangesAsync();
    }
}
