using System;
using FitnessApp.Domain.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessApp.Infrastructure.Data.Configurations;


/// <summary>
/// EF Core configuration for WorkoutExercise entity
/// </summary>
public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
{
    public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
    {
        builder.ToTable("WorkoutExercises");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.ExerciseName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e => e.Sets)
            .IsRequired();
        
        builder.Property(e => e.Reps)
            .IsRequired();
        
        builder.Property(e => e.DurationSeconds)
            .IsRequired(false);
        
        builder.Property(e => e.OrderInWorkout)
            .IsRequired();
        
        builder.Property(e => e.Notes)
            .HasMaxLength(500);
        
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        
        builder.Property(e => e.UpdatedAt)
            .IsRequired();
        
        // Relationship
        builder.HasOne(e => e.WorkoutPlan)
            .WithMany(w => w.Exercises)
            .HasForeignKey(e => e.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        builder.HasIndex(e => e.WorkoutPlanId);
        builder.HasIndex(e => e.OrderInWorkout);
    }
}
