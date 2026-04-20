using System.Threading.Tasks;
using FitnessApp.Domain.Command;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Application.Features.Workouts.Command;

/// <summary>
/// Comandă pentru adăugarea unui exercițiu în plan.
/// </summary>
public class AddExerciseCommand : IWorkoutCommand
{
    private readonly WorkoutPlan _plan;
    private readonly WorkoutExercise _exercise;

    public string Name => $"Add Exercise: {_exercise.ExerciseName}";

    public AddExerciseCommand(WorkoutPlan plan, WorkoutExercise exercise)
    {
        _plan = plan;
        _exercise = exercise;
    }

    public Task ExecuteAsync()
    {
        _plan.AddExercise(_exercise);
        return Task.CompletedTask;
    }

    public Task UndoAsync()
    {
        _plan.RemoveExercise(_exercise);
        return Task.CompletedTask;
    }

    public Task RedoAsync() => ExecuteAsync();
}

/// <summary>
/// Comandă pentru ștergerea unui exercițiu din plan.
/// </summary>
public class RemoveExerciseCommand : IWorkoutCommand
{
    private readonly WorkoutPlan _plan;
    private readonly WorkoutExercise _exercise;

    public string Name => $"Remove Exercise: {_exercise.ExerciseName}";

    public RemoveExerciseCommand(WorkoutPlan plan, WorkoutExercise exercise)
    {
        _plan = plan;
        _exercise = exercise;
    }

    public Task ExecuteAsync()
    {
        _plan.RemoveExercise(_exercise);
        return Task.CompletedTask;
    }

    public Task UndoAsync()
    {
        _plan.AddExercise(_exercise);
        return Task.CompletedTask;
    }

    public Task RedoAsync() => ExecuteAsync();
}

/// <summary>
/// Comandă pentru actualizarea unui exercițiu.
/// Salvează starea veche pentru Undo.
/// </summary>
public class UpdateExerciseCommand : IWorkoutCommand
{
    private readonly WorkoutExercise _exercise;
    private readonly int _newSets;
    private readonly int _newReps;
    
    // Backup values for Undo
    private int _oldSets;
    private int _oldReps;

    public string Name => $"Update {_exercise.ExerciseName} to {_newSets}x{_newReps}";

    public UpdateExerciseCommand(WorkoutExercise exercise, int newSets, int newReps)
    {
        _exercise = exercise;
        _newSets = newSets;
        _newReps = newReps;
    }

    public Task ExecuteAsync()
    {
        // Save current state as backup
        // Notă: În sistemul real aici am folosi un mediator sau am avea setteri publici controlați.
        // Pentru demo, folosim reflection sau expunem proprietățile dacă e necesar.
        // Având în vedere că proprietățile sunt private set, folosim Reflection simplu pentru lab.
        
        var setsProperty = typeof(WorkoutExercise).GetProperty(nameof(WorkoutExercise.Sets));
        var repsProperty = typeof(WorkoutExercise).GetProperty(nameof(WorkoutExercise.Reps));

        _oldSets = _exercise.Sets;
        _oldReps = _exercise.Reps;

        setsProperty?.SetValue(_exercise, _newSets);
        repsProperty?.SetValue(_exercise, _newReps);

        return Task.CompletedTask;
    }

    public Task UndoAsync()
    {
        var setsProperty = typeof(WorkoutExercise).GetProperty(nameof(WorkoutExercise.Sets));
        var repsProperty = typeof(WorkoutExercise).GetProperty(nameof(WorkoutExercise.Reps));

        setsProperty?.SetValue(_exercise, _oldSets);
        repsProperty?.SetValue(_exercise, _oldReps);

        return Task.CompletedTask;
    }

    public Task RedoAsync() => ExecuteAsync();
}
