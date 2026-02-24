using System;
using FitnessApp.Domain.Entities.Base;

namespace FitnessApp.Domain.Entities.Workouts;

/// <summary>
/// Exercise within a workout plan
/// </summary>
public class WorkoutExercise : BaseEntity
{
    public int WorkoutPlanId { get; private set; }
    public string ExerciseName { get; private set; }
    public int Sets { get; private set; }
    public int Reps { get; private set; }
    public int? DurationSeconds { get; private set; }
    public int OrderInWorkout { get; private set; }
    public string? Notes { get; private set; }
    
    // Navigation
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;
    
    private WorkoutExercise() : base() { }
    
    public WorkoutExercise(
        int workoutPlanId,
        string exerciseName,
        int sets,
        int reps,
        int orderInWorkout) : base()
    {
        if (workoutPlanId <= 0)
            throw new ArgumentException("WorkoutPlanId must be positive", nameof(workoutPlanId));
        
        if (string.IsNullOrWhiteSpace(exerciseName))
            throw new ArgumentException("ExerciseName cannot be empty", nameof(exerciseName));
        
        if (sets <= 0)
            throw new ArgumentException("Sets must be positive", nameof(sets));
        
        if (reps <= 0)
            throw new ArgumentException("Reps must be positive", nameof(reps));
        
        WorkoutPlanId = workoutPlanId;
        ExerciseName = exerciseName;
        Sets = sets;
        Reps = reps;
        OrderInWorkout = orderInWorkout;
    }
    
    public void SetDuration(int seconds)
    {
        if (seconds <= 0)
            throw new ArgumentException("Duration must be positive", nameof(seconds));
        
        DurationSeconds = seconds;
    }
    
    public void AddNotes(string notes)
    {
        Notes = notes;
    }
}