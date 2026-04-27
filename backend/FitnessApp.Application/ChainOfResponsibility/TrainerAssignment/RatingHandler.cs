using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public class RatingHandler : BaseTrainerHandler
    {
        public override IEnumerable<Trainer> Handle(AssignmentRequest request, IEnumerable<Trainer> trainers)
        {
            if (request.MinRating.HasValue)
            {
                trainers = trainers.Where(t => t.Rating >= request.MinRating.Value);
            }

            return base.Handle(request, trainers);
        }
    }
}
