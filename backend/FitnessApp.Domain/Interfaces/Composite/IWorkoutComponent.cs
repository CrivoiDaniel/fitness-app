using System;

namespace FitnessApp.Domain.Interfaces.Composite;


/// <summary>
/// Composite Component interface for workout structures.
/// Leaf = WorkoutExercise, Composite = WorkoutPlan (and later WorkoutDay if you add it).
/// </summary>
public interface IWorkoutComponent
{
    string DisplayName { get; }

    int GetTotalSets();
    int GetTotalReps();
    int GetTotalDurationSeconds();
}
