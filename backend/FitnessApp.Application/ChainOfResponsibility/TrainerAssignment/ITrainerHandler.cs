using System.Collections.Generic;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public interface ITrainerHandler
    {
        void SetNext(ITrainerHandler nextHandler);
        IEnumerable<Trainer> Handle(AssignmentRequest request, IEnumerable<Trainer> trainers);
    }
}
