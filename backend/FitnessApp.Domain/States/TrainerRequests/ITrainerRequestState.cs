using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Domain.States.TrainerRequests
{
    public interface ITrainerRequestState
    {
        string StatusName { get; }
        
        // Acțiuni posibile
        void Review(TrainerRequest context);
        void Accept(TrainerRequest context);
        void Reject(TrainerRequest context, string reason);
        void Cancel(TrainerRequest context);
        
        // Verificări de permisiuni
        bool CanBeCancelled();
        bool CanBeAccepted();
        bool IsFinal();
    }
}
