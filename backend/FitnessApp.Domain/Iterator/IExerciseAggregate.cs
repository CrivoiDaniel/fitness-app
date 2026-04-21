namespace FitnessApp.Domain.Iterator;

/// <summary>
/// Aggregate Interface
/// Defines the method for creating an iterator from the collection.
/// </summary>
public interface IExerciseAggregate
{
    IWorkoutIterator CreateIterator(string type = "sequential");
}
