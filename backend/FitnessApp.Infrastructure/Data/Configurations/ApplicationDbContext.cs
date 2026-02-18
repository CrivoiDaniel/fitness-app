using System;
using FitnessApp.Infrastructure.Data;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Data.Configurations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

   // ========== USER MODULE ==========
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Trainer> Trainers { get; set; } = null!;

    // ========== SUBSCRIPTION MODULE ==========
    public DbSet<Benefit> Benefits { get; set; } = null!;
    public DbSet<BenefitPackage> BenefitPackages { get; set; } = null!;
    public DbSet<BenefitPackageItem> BenefitPackageItems { get; set; } = null!;
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

}
