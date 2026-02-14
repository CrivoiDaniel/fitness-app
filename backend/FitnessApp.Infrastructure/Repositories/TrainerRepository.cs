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
}
