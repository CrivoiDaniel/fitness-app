using System;
using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Memento;

/// <summary>
/// Concrete Memento
/// Stores the full internal state of a WorkoutPlan.
/// Immutable to prevent tampering by the Caretaker.
/// </summary>
public class WorkoutPlanMemento : IWorkoutPlanMemento
{
    public string Name { get; }
    public DateTime CreatedAt { get; }

    // State to be saved
    public string PlanName { get; }
    public string? Description { get; }
    public WorkoutGoal Goal { get; }
    public DifficultyLevel Difficulty { get; }
    public int DurationWeeks { get; }
    public DayOfWeekFlag WorkoutDays { get; }
    public int SessionsPerWeek { get; }
    public int SessionDurationMinutes { get; }
    public int? RestDaysBetweenSessions { get; }
    public string? SpecialNotes { get; }
    public bool IsActive { get; }
    
    // Exercise Snapshots (using the Prototype Pattern's Clone to ensure deep copy)
    public List<WorkoutExercise> Exercises { get; }

    public WorkoutPlanMemento(string checkpointName, WorkoutPlan plan)
    {
        Name = checkpointName;
        CreatedAt = DateTime.UtcNow;

        // Capture state
        PlanName = plan.Name;
        Description = plan.Description;
        Goal = plan.Goal;
        Difficulty = plan.Difficulty;
        DurationWeeks = plan.DurationWeeks;
        WorkoutDays = plan.WorkoutDays;
        SessionsPerWeek = plan.SessionsPerWeek;
        SessionDurationMinutes = plan.SessionDurationMinutes;
        RestDaysBetweenSessions = plan.RestDaysBetweenSessions;
        SpecialNotes = plan.SpecialNotes;
        IsActive = plan.IsActive;

        // Capture deep copy of exercises
        Exercises = plan.Exercises.Select(e => e.Clone()).ToList();
    }
}
