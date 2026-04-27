using System;
using FitnessApp.Domain.Decorator;
using FitnessApp.Domain.Entities.Auth;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Entities.Appointments;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // ========== USER MODULE ==========
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Trainer> Trainers { get; set; } = null!;
    public DbSet<TrainerRequest> TrainerRequests { get; set; } = null!;

    // ========== SUBSCRIPTION MODULE ==========
    public DbSet<Benefit> Benefits { get; set; } = null!;
    public DbSet<BenefitPackage> BenefitPackages { get; set; } = null!;
    public DbSet<BenefitPackageItem> BenefitPackageItems { get; set; } = null!;
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;

    // ========== WORKOUT MODULE ==========
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; } = null!;
    public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = null!;
    
    // ========== AUTH MODULE ========== ← ADD THIS SECTION
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    
    // ========== APPOINTMENT MODULE ==========
    public DbSet<Appointment> Appointments { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is FitnessApp.Domain.Entities.Base.BaseEntity && (
                e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (FitnessApp.Domain.Entities.Base.BaseEntity)entityEntry.Entity;
            
            // Folosim reflection pentru a seta proprietățile protected
            var updatedAtProp = typeof(FitnessApp.Domain.Entities.Base.BaseEntity).GetProperty("UpdatedAt");
            updatedAtProp?.SetValue(entity, DateTime.UtcNow);

            if (entityEntry.State == EntityState.Added)
            {
                var createdAtProp = typeof(FitnessApp.Domain.Entities.Base.BaseEntity).GetProperty("CreatedAt");
                createdAtProp?.SetValue(entity, DateTime.UtcNow);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}