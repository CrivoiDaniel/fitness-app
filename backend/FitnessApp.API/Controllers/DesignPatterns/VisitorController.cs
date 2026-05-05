using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.Visitor;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Entities.Workouts;
using System;
using System.Collections.Generic;

namespace FitnessApp.API.Controllers.DesignPatterns
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : ControllerBase
    {
        [HttpGet("analyze")]
        public IActionResult Analyze()
        {
            // 1. Pregătim structura de obiecte eterogene (Elementele)
            var elements = new List<dynamic>();

            // Mocking data for demo
            var client = new Client(1, DateTime.UtcNow.AddYears(-25));
            var trainer = new Trainer(2, "Crossfit", 8);
            trainer.UpdateRating(4.8m);
            
            var plan = new WorkoutPlan("Bulking Phase", 1, Domain.Enums.WorkoutGoal.MuscleGain, Domain.Enums.DifficultyLevel.Intermediate, 12, Domain.Enums.DayOfWeekFlag.Monday, 4, 60);

            elements.Add(client);
            elements.Add(trainer);
            elements.Add(plan);

            // 2. Creăm Vizitatorul
            var visitor = new PerformanceScoreVisitor();

            // 3. Vizităm fiecare element (Double Dispatch)
            foreach (var element in elements)
            {
                element.Accept(visitor);
            }

            // 4. Returnăm rezultatele acumulate de vizitator
            return Ok(visitor.Results);
        }
    }
}
