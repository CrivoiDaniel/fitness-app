using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Domain.Entities.Users
{
    public class TrainerRequest : BaseEntity
    {
        public int ClientId { get; private set; }
        public Client Client { get; private set; } = null!;

        public int TrainerId { get; private set; }
        public Trainer Trainer { get; private set; } = null!;

        public string Status { get; private set; } // Submitted, UnderReview, Accepted, Rejected, Cancelled
        public string? Message { get; private set; }
        public string? RejectionReason { get; private set; }

        private States.TrainerRequests.ITrainerRequestState? _state;

        private States.TrainerRequests.ITrainerRequestState CurrentState
        {
            get
            {
                if (_state == null)
                {
                    _state = Status switch
                    {
                        "Submitted" => new States.TrainerRequests.SubmittedState(),
                        "Pending" => new States.TrainerRequests.SubmittedState(),
                        "UnderReview" => new States.TrainerRequests.UnderReviewState(),
                        "Accepted" => new States.TrainerRequests.AcceptedState(),
                        "Rejected" => new States.TrainerRequests.RejectedState(RejectionReason ?? ""),
                        "Cancelled" => new States.TrainerRequests.CancelledState(),
                        _ => new States.TrainerRequests.SubmittedState()
                    };
                }
                return _state;
            }
        }

        protected TrainerRequest() : base() { }

        public TrainerRequest(int clientId, int trainerId, string? message = null) : base()
        {
            ClientId = clientId;
            TrainerId = trainerId;
            Message = message;
            Status = "Submitted";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void StartReview() => CurrentState.Review(this);
        public void Accept() => CurrentState.Accept(this);
        public void Reject(string reason) => CurrentState.Reject(this, reason);
        public void Cancel() => CurrentState.Cancel(this);

        public void TransitionTo(States.TrainerRequests.ITrainerRequestState newState)
        {
            _state = newState;
            Status = newState.StatusName;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanBeCancelled => CurrentState.CanBeCancelled();
        public bool IsFinal => CurrentState.IsFinal();
    }
}
