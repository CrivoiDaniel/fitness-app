namespace FitnessApp.Domain.Flyweight;

/// <summary>
/// Flyweight: intrinsic (shared) exercise definition.
/// </summary>
public sealed class ExerciseFlyweight
{
    public string Name { get; }
    public string? MuscleGroup { get; }
    public string? Equipment { get; }

    public ExerciseFlyweight(string name, string? muscleGroup, string? equipment)
    {
        Name = name;
        MuscleGroup = muscleGroup;
        Equipment = equipment;
    }
}