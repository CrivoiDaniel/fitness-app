using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.Domain.Command;

namespace FitnessApp.Application.Features.Workouts.Command;

/// <summary>
/// Clasa Invoker pentru pattern-ul Command.
/// Gestionează istoricul comenzilor pentru a permite Undo/Redo.
/// </summary>
public class WorkoutPlanEditor
{
    private readonly Stack<IWorkoutCommand> _undoStack = new();
    private readonly Stack<IWorkoutCommand> _redoStack = new();

    /// <summary>
    /// Execută o comandă și o adaugă în istoric.
    /// </summary>
    public async Task ExecuteCommandAsync(IWorkoutCommand command)
    {
        await command.ExecuteAsync();
        _undoStack.Push(command);
        
        // Resetăm stiva de redo când executăm o comandă nouă
        _redoStack.Clear();
        
        System.Console.WriteLine($"[COMMAND EXECUTED] {command.Name}");
    }

    /// <summary>
    /// Revine la starea anterioară.
    /// </summary>
    public async Task UndoAsync()
    {
        if (_undoStack.Count == 0) return;

        var command = _undoStack.Pop();
        await command.UndoAsync();
        _redoStack.Push(command);
        
        System.Console.WriteLine($"[COMMAND UNDO] {command.Name}");
    }

    /// <summary>
    /// Repetă ultima comandă anulată.
    /// </summary>
    public async Task RedoAsync()
    {
        if (_redoStack.Count == 0) return;

        var command = _redoStack.Pop();
        await command.RedoAsync();
        _undoStack.Push(command);
        
        System.Console.WriteLine($"[COMMAND REDO] {command.Name}");
    }

    public IEnumerable<string> GetUndoHistory()
    {
        foreach (var cmd in _undoStack) yield return cmd.Name;
    }

    public IEnumerable<string> GetRedoHistory()
    {
        foreach (var cmd in _redoStack) yield return cmd.Name;
    }

    public bool CanUndo => _undoStack.Count > 0;
    public bool CanRedo => _redoStack.Count > 0;
}
