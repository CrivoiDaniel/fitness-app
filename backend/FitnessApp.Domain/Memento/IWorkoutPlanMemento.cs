using System;

namespace FitnessApp.Domain.Memento;

/// <summary>
/// Memento Interface
/// Lets the caretaker work with the memento's metadata (Name, Date).
/// </summary>
public interface IWorkoutPlanMemento
{
    string Name { get; }
    DateTime CreatedAt { get; }
}
