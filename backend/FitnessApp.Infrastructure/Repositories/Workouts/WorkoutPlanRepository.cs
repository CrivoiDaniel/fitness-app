using System;
using FitnessApp.Application.Interfaces.Workout;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Workouts;

public class WorkoutPlanRepository : IWorkoutPlanRepository
{
    private readonly ApplicationDbContext _context;
    
    public WorkoutPlanRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<WorkoutPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }
    
    public async Task<WorkoutPlan?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(w => w.Client)
                .ThenInclude(c => c.User)
            .Include(w => w.Trainer)
                .ThenInclude(t => t.User)
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }
    
    public async Task<List<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(w => w.Client)
                .ThenInclude(c => c.User)
            .Include(w => w.Trainer)
                .ThenInclude(t => t.User)
            .Include(w => w.Exercises)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<WorkoutPlan>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(w => w.Client)
                .ThenInclude(c => c.User)
            .Include(w => w.Exercises)
            .Where(w => w.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<List<WorkoutPlan>> GetByTrainerIdAsync(int trainerId, CancellationToken cancellationToken = default)
    {
        return await _context.WorkoutPlans
            .Include(w => w.Exercises)
            .Where(w => w.TrainerId == trainerId)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default)
    {
        await _context.WorkoutPlans.AddAsync(workoutPlan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return workoutPlan;
    }
    
    public async Task UpdateAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default)
    {
        _context.WorkoutPlans.Update(workoutPlan);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var workoutPlan = await GetByIdAsync(id, cancellationToken);
        if (workoutPlan != null)
        {
            _context.WorkoutPlans.Remove(workoutPlan);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}