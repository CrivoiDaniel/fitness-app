using FitnessApp.Domain.Flyweight;

namespace FitnessApp.Application.Interfaces.Flyweight;

public interface IExerciseCatalog
{
    ExerciseFlyweight Get(string exerciseName);
}