using System;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        Console.WriteLine(">>> [SEEDER] STARTING...");

        // 1. Seed Admin
        var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@fitness.com");
        if (adminUser == null)
        {
            Console.WriteLine(">>> [SEEDER] Creating Admin...");
            adminUser = new User(
                firstName: "admin",
                lastName: "admin",
                email: "admin@fitness.com",
                passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin@123", 12),
                role: Role.Admin
            );
            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // 2. Seed / Reset Trainer Ion
        var ionUser = await context.Users.Include(u => u.TrainerProfile).FirstOrDefaultAsync(u => u.Email == "ion@gmail.com");
        
        string hashedPass = BCrypt.Net.BCrypt.HashPassword("ion123", 12);

        if (ionUser == null)
        {
            Console.WriteLine(">>> [SEEDER] Creating Ion user...");
            ionUser = new User(
                firstName: "Ion",
                lastName: "Antrenorul",
                email: "ion@gmail.com",
                passwordHash: hashedPass,
                role: Role.Trainer
            );
            await context.Users.AddAsync(ionUser);
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine(">>> [SEEDER] Ion exists. Resetting password and role...");
            // Force reset to ensure we can log in with ion123
            ionUser.SetPasswordHash(hashedPass);
            
            // Hack to change role if it's private
            var roleProperty = typeof(User).GetProperty("Role");
            if (roleProperty != null) roleProperty.SetValue(ionUser, Role.Trainer);
            
            await context.SaveChangesAsync();
        }

        if (ionUser.TrainerProfile == null)
        {
            Console.WriteLine(">>> [SEEDER] Creating Ion Trainer profile...");
            var ionTrainer = new Trainer(ionUser.Id, "Fitness & Bodybuilding", 8);
            await context.Trainers.AddAsync(ionTrainer);
            await context.SaveChangesAsync();
        }

        Console.WriteLine(">>> [SEEDER] COMPLETED.");
    }
}