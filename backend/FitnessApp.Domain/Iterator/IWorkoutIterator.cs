using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Domain.Iterator;

/// <summary>
/// Iterator Interface
/// Standard operations for traversing a collection.
/// </summary>
public interface IWorkoutIterator
{
    WorkoutExercise? GetNext();
    bool HasMore();
    void Reset();
    int CurrentPosition { get; }
    WorkoutExercise? Current();
}
