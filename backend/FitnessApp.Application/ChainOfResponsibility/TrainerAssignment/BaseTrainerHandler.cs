using System.Collections.Generic;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public abstract class BaseTrainerHandler : ITrainerHandler
    {
        private ITrainerHandler? _nextHandler;

        public void SetNext(ITrainerHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        public virtual IEnumerable<Trainer> Handle(AssignmentRequest request, IEnumerable<Trainer> trainers)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(request, trainers);
            }

            return trainers;
        }
    }
}
