using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Domain.Command;

/// <summary>
/// Interfața de bază pentru pattern-ul Command.
/// Permite încapsularea unor acțiuni asupra planurilor de antrenament.
/// </summary>
public interface IWorkoutCommand
{
    /// <summary>
    /// Numele sugestiv al comenzii (pentru UI/Istoric).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Execută acțiunea principală.
    /// </summary>
    Task ExecuteAsync();

    /// <summary>
    /// Revine la starea anterioară executării comenzii.
    /// </summary>
    Task UndoAsync();

    /// <summary>
    /// Repetă acțiunea după un Undo.
    /// </summary>
    Task RedoAsync();
}
