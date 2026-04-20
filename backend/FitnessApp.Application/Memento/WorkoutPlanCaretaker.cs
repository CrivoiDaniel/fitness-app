using System.Collections.Generic;
using FitnessApp.Domain.Memento;

namespace FitnessApp.Application.Memento;

/// <summary>
/// Memento Caretaker
/// Manages the history of workout plan snapshots (Checkpoints).
/// Does not know the internal details of the snapshots.
/// </summary>
public class WorkoutPlanCaretaker
{
    private readonly List<IWorkoutPlanMemento> _mementos = new();

    public void AddMemento(IWorkoutPlanMemento memento)
    {
        _mementos.Add(memento);
    }

    public IWorkoutPlanMemento? GetMemento(int index)
    {
        if (index < 0 || index >= _mementos.Count)
            return null;
            
        return _mementos[index];
    }

    public List<IWorkoutPlanMemento> GetAllMementos()
    {
        return new List<IWorkoutPlanMemento>(_mementos);
    }

    public void Clear()
    {
        _mementos.Clear();
    }

    public int Count => _mementos.Count;
}
