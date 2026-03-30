using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Application.Bridge;

// ABSTRACȚIE RAFINATĂ 1
public class SimpleWorkoutExporter : WorkoutExporter
{
    public SimpleWorkoutExporter(IExportFormat exportFormat) : base(exportFormat) { }
    public override byte[] GenerateExport(WorkoutPlan workout)
    {
        _exportFormat.AddTitle($"Workout Plan: {workout.Name}");
        _exportFormat.AddSubtitle($"Goal: {workout.Goal} | Difficulty: {workout.Difficulty}");
        
        var summaryText = $"This is a {workout.DurationWeeks} weeks program. " +
                          $"You will train {workout.SessionsPerWeek} days a week for {workout.SessionDurationMinutes} minutes per session.";
        
        _exportFormat.AddText(summaryText);
        return _exportFormat.GetFileBytes();
    }
}