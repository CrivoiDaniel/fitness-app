using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Domain.States.TrainerRequests
{
    // 1. Starea: Trimisă (Inițială)
    public class SubmittedState : ITrainerRequestState
    {
        public string StatusName => "Submitted";

        public void Review(TrainerRequest context) => context.TransitionTo(new UnderReviewState());
        public void Accept(TrainerRequest context) => context.TransitionTo(new AcceptedState());
        public void Reject(TrainerRequest context, string reason) => context.TransitionTo(new RejectedState(reason));
        public void Cancel(TrainerRequest context) => context.TransitionTo(new CancelledState());

        public bool CanBeCancelled() => true;
        public bool CanBeAccepted() => true;
        public bool IsFinal() => false;
    }

    // 2. Starea: În Analiză
    public class UnderReviewState : ITrainerRequestState
    {
        public string StatusName => "UnderReview";

        public void Review(TrainerRequest context) { /* Deja în analiză */ }
        public void Accept(TrainerRequest context) => context.TransitionTo(new AcceptedState());
        public void Reject(TrainerRequest context, string reason) => context.TransitionTo(new RejectedState(reason));
        public void Cancel(TrainerRequest context) 
        {
            throw new InvalidOperationException("Cererea este deja în analiză de către antrenor și nu mai poate fi anulată direct.");
        }

        public bool CanBeCancelled() => false;
        public bool CanBeAccepted() => true;
        public bool IsFinal() => false;
    }

    // 3. Starea: Acceptată (Finală Pozitivă)
    public class AcceptedState : ITrainerRequestState
    {
        public string StatusName => "Accepted";

        public void Review(TrainerRequest context) => throw new InvalidOperationException("Cererea a fost deja acceptată.");
        public void Accept(TrainerRequest context) { }
        public void Reject(TrainerRequest context, string reason) => throw new InvalidOperationException("O cerere acceptată nu mai poate fi respinsă.");
        public void Cancel(TrainerRequest context) => throw new InvalidOperationException("Cererea a fost deja acceptată.");

        public bool CanBeCancelled() => false;
        public bool CanBeAccepted() => false;
        public bool IsFinal() => true;
    }

    // 4. Starea: Respinsă (Finală Negativă)
    public class RejectedState : ITrainerRequestState
    {
        private readonly string _reason;
        public string StatusName => "Rejected";

        public RejectedState(string reason) => _reason = reason;

        public void Review(TrainerRequest context) => throw new InvalidOperationException("Cererea a fost deja respinsă.");
        public void Accept(TrainerRequest context) => throw new InvalidOperationException("O cerere respinsă nu poate fi acceptată fără re-trimitere.");
        public void Reject(TrainerRequest context, string reason) { }
        public void Cancel(TrainerRequest context) { }

        public bool CanBeCancelled() => false;
        public bool CanBeAccepted() => false;
        public bool IsFinal() => true;
    }

    // 5. Starea: Anulată (De către Client)
    public class CancelledState : ITrainerRequestState
    {
        public string StatusName => "Cancelled";

        public void Review(TrainerRequest context) => throw new InvalidOperationException("Cererea a fost anulată.");
        public void Accept(TrainerRequest context) => throw new InvalidOperationException("Cererea a fost anulată.");
        public void Reject(TrainerRequest context, string reason) => throw new InvalidOperationException("Cererea a fost anulată.");
        public void Cancel(TrainerRequest context) { }

        public bool CanBeCancelled() => false;
        public bool CanBeAccepted() => false;
        public bool IsFinal() => true;
    }
}
