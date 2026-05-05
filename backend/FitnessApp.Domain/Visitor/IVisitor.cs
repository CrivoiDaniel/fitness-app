using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Domain.Visitor
{
    public interface IVisitor
    {
        void Visit(Client client);
        void Visit(Trainer trainer);
        void Visit(WorkoutPlan workoutPlan);
    }
}
