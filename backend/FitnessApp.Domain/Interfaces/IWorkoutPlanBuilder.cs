using System;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Interfaces;


/// <summary>
/// Builder interface that declares product construction steps
/// Common to all types of workout plan builders
/// </summary>
public interface IWorkoutPlanBuilder
{
    // ========== REQUIRED CONFIGURATION ==========
    
    IWorkoutPlanBuilder WithName(string name);
    IWorkoutPlanBuilder ForClient(int clientId);
    IWorkoutPlanBuilder WithGoal(WorkoutGoal goal);
    IWorkoutPlanBuilder WithDifficulty(DifficultyLevel difficulty);
    IWorkoutPlanBuilder WithDuration(int weeks);
    IWorkoutPlanBuilder OnDays(params DayOfWeek[] days);
    IWorkoutPlanBuilder OnDays(DayOfWeekFlag days);
    IWorkoutPlanBuilder WithSessionDuration(int minutes);
    
    // ========== OPTIONAL CONFIGURATION ==========
    
    IWorkoutPlanBuilder WithTrainer(int trainerId);
    IWorkoutPlanBuilder WithDescription(string description);
    IWorkoutPlanBuilder WithRestDays(int days);
    IWorkoutPlanBuilder WithNotes(string notes);
    
    // ========== EXERCISES ==========
    
    IWorkoutPlanBuilder AddExercise(string name, int sets, int reps, int? durationSeconds = null, string? notes = null);
    
    // ========== BUILD & RESET ==========
    
    WorkoutPlan Build();
    IWorkoutPlanBuilder Reset();
}