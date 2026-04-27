using System;
using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public class SpecializationHandler : BaseTrainerHandler
    {
        public override IEnumerable<Trainer> Handle(AssignmentRequest request, IEnumerable<Trainer> trainers)
        {
            if (!string.IsNullOrEmpty(request.PreferredSpecialization))
            {
                trainers = trainers.Where(t => t.Specialization.Contains(request.PreferredSpecialization, StringComparison.OrdinalIgnoreCase));
            }

            return base.Handle(request, trainers);
        }
    }
}
