using System.Collections.Generic;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public class TrainerAssignmentService
    {
        private readonly ITrainerHandler _chain;

        public TrainerAssignmentService()
        {
            // Build the chain
            var specHandler = new SpecializationHandler();
            var expHandler = new ExperienceHandler();
            var rateHandler = new RatingHandler();

            specHandler.SetNext(expHandler);
            expHandler.SetNext(rateHandler);

            _chain = specHandler;
        }

        public IEnumerable<Trainer> FindTrainers(AssignmentRequest request, IEnumerable<Trainer> trainers)
        {
            return _chain.Handle(request, trainers);
        }
    }
}
