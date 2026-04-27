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

        public string Status { get; private set; } // Pending, Accepted, Rejected
        public string? Message { get; private set; }

        protected TrainerRequest() : base() { }

        public TrainerRequest(int clientId, int trainerId, string? message = null) : base()
        {
            ClientId = clientId;
            TrainerId = trainerId;
            Message = message;
            Status = "Pending";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Accept()
        {
            Status = "Accepted";
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = "Rejected";
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
