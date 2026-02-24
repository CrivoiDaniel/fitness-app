using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Entities.Workouts;

/// <summary>
/// Workout Plan entity
/// Represents a structured workout program for a client
/// </summary>
public class WorkoutPlan : BaseEntity
{
    // Basic Info
    public string Name { get; private set; }
    public string? Description { get; private set; }
    
    // Relationships
    public int ClientId { get; private set; }
    public int? TrainerId { get; private set; }
    
    // Configuration
    public WorkoutGoal Goal { get; private set; }
    public DifficultyLevel Difficulty { get; private set; }
    public int DurationWeeks { get; private set; }
    
    // Schedule
    public DayOfWeekFlag WorkoutDays { get; private set; }
    public int SessionsPerWeek { get; private set; }
    public int SessionDurationMinutes { get; private set; }
    
    // Optional
    public int? RestDaysBetweenSessions { get; private set; }
    public string? SpecialNotes { get; private set; }
    public bool IsActive { get; private set; }
    
    // Navigation Properties
    public virtual Client Client { get; set; } = null!;
    public virtual Trainer? Trainer { get; set; }
    public virtual ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    
    // Private constructor for EF Core
    private WorkoutPlan() : base() { }
    
    // Public constructor with validation
    public WorkoutPlan(
        string name,
        int clientId,
        WorkoutGoal goal,
        DifficultyLevel difficulty,
        int durationWeeks,
        DayOfWeekFlag workoutDays,
        int sessionsPerWeek,
        int sessionDurationMinutes) : base()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (clientId <= 0)
            throw new ArgumentException("ClientId must be positive", nameof(clientId));
        
        if (durationWeeks <= 0)
            throw new ArgumentException("Duration must be at least 1 week", nameof(durationWeeks));
        
        if (sessionsPerWeek <= 0 || sessionsPerWeek > 7)
            throw new ArgumentException("Sessions per week must be between 1 and 7", nameof(sessionsPerWeek));
        
        if (sessionDurationMinutes <= 0)
            throw new ArgumentException("Session duration must be positive", nameof(sessionDurationMinutes));
        
        Name = name;
        ClientId = clientId;
        Goal = goal;
        Difficulty = difficulty;
        DurationWeeks = durationWeeks;
        WorkoutDays = workoutDays;
        SessionsPerWeek = sessionsPerWeek;
        SessionDurationMinutes = sessionDurationMinutes;
        IsActive = true;
    }
    
    // PUBLIC METHODS
    
    public void SetDescription(string? description)
    {
        Description = description;
    }
    
    public void AssignTrainer(int trainerId)
    {
        if (trainerId <= 0)
            throw new ArgumentException("TrainerId must be positive", nameof(trainerId));
        
        TrainerId = trainerId;
    }
    
    public void SetRestDays(int restDays)
    {
        if (restDays < 0 || restDays > 7)
            throw new ArgumentException("Rest days must be between 0 and 7", nameof(restDays));
        
        RestDaysBetweenSessions = restDays;
    }
    
    public void AddSpecialNotes(string notes)
    {
        SpecialNotes = notes;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void AddExercise(WorkoutExercise exercise)
    {
        if (exercise == null)
            throw new ArgumentNullException(nameof(exercise));
        
        Exercises.Add(exercise);
    }
    
    // COMPUTED PROPERTIES
    
    public int TotalSessions()
    {
        return DurationWeeks * SessionsPerWeek;
    }
    
    public int TotalMinutes()
    {
        return TotalSessions() * SessionDurationMinutes;
    }
    
    public bool HasTrainer()
    {
        return TrainerId.HasValue && TrainerId.Value > 0;
    }
    
    public int ExerciseCount()
    {
        return Exercises?.Count ?? 0;
    }
}