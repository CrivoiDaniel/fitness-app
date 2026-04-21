using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Domain.Iterator;

/// <summary>
/// Concrete Iterator: Sequential
/// Traverses exercises in their standard order.
/// </summary>
public class SequentialWorkoutIterator : IWorkoutIterator
{
    private readonly List<WorkoutExercise> _items;
    private int _position = -1;

    public int CurrentPosition => _position;

    public SequentialWorkoutIterator(IEnumerable<WorkoutExercise> items)
    {
        _items = items.OrderBy(e => e.OrderInWorkout).ToList();
    }

    public WorkoutExercise? GetNext()
    {
        if (HasMore())
        {
            _position++;
            return _items[_position];
        }
        return null;
    }

    public bool HasMore()
    {
        return _position < _items.Count - 1;
    }

    public void Reset()
    {
        _position = -1;
    }

    public WorkoutExercise? Current()
    {
        if (_position >= 0 && _position < _items.Count)
            return _items[_position];
        return null;
    }
}
