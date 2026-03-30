using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Application.Bridge;

// ABSTRACȚIE RAFINATĂ 2
public class DetailedWorkoutExporter : WorkoutExporter
{
    public DetailedWorkoutExporter(IExportFormat exportFormat) : base(exportFormat) { }
    public override byte[] GenerateExport(WorkoutPlan workout)
    {
        _exportFormat.AddTitle($"Detailed Plan: {workout.Name}");
        _exportFormat.AddSubtitle($"Goal: {workout.Goal}");
        
        if (!string.IsNullOrEmpty(workout.Description))
            _exportFormat.AddText(workout.Description);
        _exportFormat.AddText($"Stats: {workout.TotalSessions()} total sessions, {workout.GetTotalDurationSeconds() / 60} total minutes of work.");
        // Aici am corectat proprietățile pe baza fișierului tău WorkoutExercise.cs:
        // Am înlocuit "DayOfWeek" cu "OrderInWorkout" 
        // Am înlocuit "Name" cu "ExerciseName"
        var exercisesList = workout.Exercises
            .OrderBy(e => e.OrderInWorkout)
            .Select(e => $"Order {e.OrderInWorkout} - {e.ExerciseName} ({e.GetTotalSets()} sets x {e.GetTotalReps()} total reps)")
            .ToList();
        if (exercisesList.Any())
        {
            _exportFormat.AddSubtitle("Exercises:");
            _exportFormat.AddList(exercisesList);
        }
        return _exportFormat.GetFileBytes();
    }
}
