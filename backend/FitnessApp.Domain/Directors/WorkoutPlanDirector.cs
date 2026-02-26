using System;
using FitnessApp.Domain.Enums;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Domain.Directors;

/// <summary>
/// Director class for WorkoutPlan construction
/// Defines the ORDER of building steps for common workout plan configurations
/// Separates construction orchestration from the Builder implementation
/// The Director works with any builder instance that implements IWorkoutPlanBuilder
/// </summary>
public class WorkoutPlanDirector
{
    private IWorkoutPlanBuilder _builder;
    
    /// <summary>
    /// Constructor accepts a builder instance
    /// </summary>
    public WorkoutPlanDirector(IWorkoutPlanBuilder builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }
    
    /// <summary>
    /// Changes the builder instance used by the director
    /// Allows using different builders with the same director
    /// </summary>
    public void SetBuilder(IWorkoutPlanBuilder builder)
    {
        _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    }
    
    // ========== PRESET CONSTRUCTIONS ==========
    
    /// <summary>
    /// Constructs a Beginner Full Body workout plan
    /// Director defines the EXACT ORDER and STEPS
    /// </summary>
    public void ConstructBeginnerFullBody(int clientId)
    {
        _builder.Reset();
        _builder
            .WithName("Beginner Full Body Workout")
            .ForClient(clientId)
            .WithGoal(WorkoutGoal.GeneralFitness)
            .WithDifficulty(DifficultyLevel.Beginner)
            .WithDuration(8)
            .OnDays(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday)
            .WithSessionDuration(45)
            .WithRestDays(1);
    }
    
    /// <summary>
    /// Constructs an Intermediate Strength training program
    /// </summary>
    public void ConstructIntermediateStrength(int clientId)
    {
        _builder.Reset();
        _builder
            .WithName("Intermediate Strength Training")
            .ForClient(clientId)
            .WithGoal(WorkoutGoal.Strength)
            .WithDifficulty(DifficultyLevel.Intermediate)
            .WithDuration(12)
            .OnDays(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday, DayOfWeek.Friday)
            .WithSessionDuration(60)
            .WithRestDays(1);
    }
    
    /// <summary>
    /// Constructs an Advanced Muscle Gain program
    /// </summary>
    public void ConstructAdvancedMuscleGain(int clientId)
    {
        _builder.Reset();
        _builder
            .WithName("Advanced Muscle Building Program")
            .ForClient(clientId)
            .WithGoal(WorkoutGoal.MuscleGain)
            .WithDifficulty(DifficultyLevel.Advanced)
            .WithDuration(16)
            .OnDays(DayOfWeekFlag.Weekdays)
            .WithSessionDuration(90)
            .WithRestDays(0);
    }
    
    /// <summary>
    /// Constructs a Weight Loss transformation program
    /// </summary>
    public void ConstructWeightLossProgram(int clientId)
    {
        _builder.Reset();
        _builder
            .WithName("Weight Loss Transformation")
            .ForClient(clientId)
            .WithGoal(WorkoutGoal.WeightLoss)
            .WithDifficulty(DifficultyLevel.Intermediate)
            .WithDuration(12)
            .OnDays(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                    DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday)
            .WithSessionDuration(50)
            .WithRestDays(0);
    }
    
    /// <summary>
    /// Constructs an Endurance/Cardio program
    /// </summary>
    public void ConstructEnduranceProgram(int clientId)
    {
        _builder.Reset();
        _builder
            .WithName("Endurance & Cardio Builder")
            .ForClient(clientId)
            .WithGoal(WorkoutGoal.Endurance)
            .WithDifficulty(DifficultyLevel.Intermediate)
            .WithDuration(10)
            .OnDays(DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday)
            .WithSessionDuration(60)
            .WithRestDays(1);
    }
}