using System;

namespace FitnessApp.Application.ChainOfResponsibility.TrainerAssignment
{
    public class AssignmentRequest
    {
        public string? PreferredSpecialization { get; set; }
        public int? MinYearsOfExperience { get; set; }
        public decimal? MinRating { get; set; }

        public override string ToString()
        {
            return $"Request: Specialization={PreferredSpecialization ?? "Any"}, MinExp={MinYearsOfExperience ?? 0}, MinRating={MinRating ?? 0}";
        }
    }
}
