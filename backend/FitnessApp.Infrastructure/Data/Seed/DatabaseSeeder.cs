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
        if (await context.Users.AnyAsync(u => u.Email == "admin@fitness.com"))
            return;

        var adminUser = new User(
            firstName: "admin",
            lastName: "admin",
            email: "admin@fitness.com",
            passwordHash: BCrypt.Net.BCrypt.HashPassword("Admin@123", 12),
            role: Role.Admin
        );

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
}