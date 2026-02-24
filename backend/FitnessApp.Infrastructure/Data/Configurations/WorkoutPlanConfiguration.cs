using System;
using FitnessApp.Domain.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for WorkoutPlan entity
/// </summary>
public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> builder)
    {
        builder.ToTable("WorkoutPlans");
        
        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(w => w.Description)
            .HasMaxLength(1000);
        
        builder.Property(w => w.Goal)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(w => w.Difficulty)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(w => w.DurationWeeks)
            .IsRequired();
        
        builder.Property(w => w.WorkoutDays)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(w => w.SessionsPerWeek)
            .IsRequired();
        
        builder.Property(w => w.SessionDurationMinutes)
            .IsRequired();
        
        builder.Property(w => w.RestDaysBetweenSessions)
            .IsRequired(false);
        
        builder.Property(w => w.SpecialNotes)
            .HasMaxLength(500);
        
        builder.Property(w => w.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(w => w.CreatedAt)
            .IsRequired();
        
        builder.Property(w => w.UpdatedAt)
            .IsRequired();
        
        // Relationships
        builder.HasOne(w => w.Client)
            .WithMany()
            .HasForeignKey(w => w.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(w => w.Trainer)
            .WithMany()
            .HasForeignKey(w => w.TrainerId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
        
        builder.HasMany(w => w.Exercises)
            .WithOne(e => e.WorkoutPlan)
            .HasForeignKey(e => e.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(w => w.ClientId);
        builder.HasIndex(w => w.TrainerId);
        builder.HasIndex(w => w.IsActive);
        builder.HasIndex(w => w.CreatedAt);
    }
}