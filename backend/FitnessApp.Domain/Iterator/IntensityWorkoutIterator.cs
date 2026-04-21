using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Domain.Iterator;

/// <summary>
/// Concrete Iterator: Intensity
/// Traverses exercises by total effort (Sets * Reps) descending.
/// </summary>
public class IntensityWorkoutIterator : IWorkoutIterator
{
    private readonly List<WorkoutExercise> _items;
    private int _position = -1;

    public int CurrentPosition => _position;

    public IntensityWorkoutIterator(IEnumerable<WorkoutExercise> items)
    {
        // Sort by Volume: Sets * Reps (descending)
        _items = items.OrderByDescending(e => e.Sets * e.Reps).ToList();
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
