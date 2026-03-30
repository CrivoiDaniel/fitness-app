using FitnessApp.Domain.Flyweight;

namespace FitnessApp.Application.Interfaces.Flyweight;

/// <summary>
/// Simple in-memory catalog for exercise meta.
/// Uses FlyweightFactory to share intrinsic definitions.
/// </summary>
public sealed class InMemoryExerciseCatalog : IExerciseCatalog
{
    private readonly ExerciseFlyweightFactory _factory = new();

    private readonly Dictionary<string, (string? MuscleGroup, string? Equipment)> _meta
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Squat"] = ("Legs", "Barbell"),
            ["Bench Press"] = ("Chest", "Barbell"),
            ["Deadlift"] = ("Back", "Barbell"),
            ["Pull-up"] = ("Back", "Bodyweight"),
            ["Push-up"] = ("Chest", "Bodyweight"),
            ["Plank"] = ("Core", "Bodyweight"),
            ["Shoulder Press"] = ("Shoulders", "Dumbbells"),
            ["Bicep Curl"] = ("Arms", "Dumbbells"),
            ["Tricep Dips"] = ("Arms", "Bodyweight"),
            ["Lunges"] = ("Legs", "Bodyweight"),
        };

    public ExerciseFlyweight Get(string exerciseName)
    {
        _meta.TryGetValue(exerciseName ?? "", out var m);
        return _factory.Get(exerciseName, m.MuscleGroup, m.Equipment);
    }
}