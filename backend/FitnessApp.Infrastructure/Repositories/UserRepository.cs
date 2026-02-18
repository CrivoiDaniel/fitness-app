using System;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Infrastructure.Data;
using FitnessApp.Infrastructure.Data.Configurations;

namespace FitnessApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return  await _context.Users
            .Include(u => u.TrainerProfile)
            .Include(u => u.ClientProfile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.TrainerProfile)
            .Include(u => u.ClientProfile)
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());
    }
    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}
