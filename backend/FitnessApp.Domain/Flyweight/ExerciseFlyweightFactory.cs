using System.Collections.Concurrent;

namespace FitnessApp.Domain.Flyweight;

/// <summary>
/// Flyweight Factory: caches/reuses flyweights by normalized exercise name.
/// </summary>
public sealed class ExerciseFlyweightFactory
{
    private readonly ConcurrentDictionary<string, ExerciseFlyweight> _cache = new();
    public int UniqueCount => _cache.Count;

    public ExerciseFlyweight Get(string name, string? muscleGroup, string? equipment)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Exercise name cannot be empty.", nameof(name));

        var key = Normalize(name);

        // Create once per key; subsequent calls reuse same instance.
        return _cache.GetOrAdd(key, _ => new ExerciseFlyweight(
            name: name.Trim(),
            muscleGroup: muscleGroup,
            equipment: equipment
        ));
    }

    private static string Normalize(string s) => s.Trim().ToUpperInvariant();
}