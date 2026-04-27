using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public class ExperienceHandler : BaseTrainerHandler
    {
        public override IEnumerable<Trainer> Handle(AssignmentRequest request, IEnumerable<Trainer> trainers)
        {
            if (request.MinYearsOfExperience.HasValue)
            {
                trainers = trainers.Where(t => t.YearsOfExperience >= request.MinYearsOfExperience.Value);
            }

            return base.Handle(request, trainers);
        }
    }
}
