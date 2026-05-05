using System;
using System.Collections.Generic;
using System.Linq;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Visitor;

namespace FitnessApp.Application.Visitor
{
    public class PerformanceScoreVisitor : IVisitor
    {
        public List<VisitorResult> Results { get; } = new();

        public void Visit(Client client)
        {
            // Logica pentru Client: Scor bazat pe abonamente active și vârstă
            double score = 50.0; // Scorul de bază
            
            // +10 puncte pentru fiecare abonament
            score += client.Subscriptions.Count * 10;
            
            // Penalizare sau bonus bazat pe vârstă (simulare)
            var age = DateTime.UtcNow.Year - client.DateOfBirth.Year;
            if (age >= 18 && age <= 40) score += 15;
            
            Results.Add(new VisitorResult 
            { 
                EntityName = $"Client: {client.User?.FirstName ?? "N/A"}", 
                Score = Math.Min(score, 100),
                Category = "Health & Consistency",
                Analysis = $"Clientul are {client.Subscriptions.Count} abonamente active. Vârsta: {age} ani."
            });
        }

        public void Visit(Trainer trainer)
        {
            // Logica pentru Trainer: Scor bazat pe experiență și rating
            double score = (double)trainer.Rating * 10; // Rating 5 -> 50 puncte
            
            // +5 puncte pentru fiecare an de experiență
            score += trainer.YearsOfExperience * 5;
            
            Results.Add(new VisitorResult 
            { 
                EntityName = $"Trainer: {trainer.User?.FirstName ?? "N/A"}", 
                Score = Math.Min(score, 100),
                Category = "Professionalism",
                Analysis = $"Antrenorul are {trainer.YearsOfExperience} ani experiență și un rating de {trainer.Rating}."
            });
        }

        public void Visit(WorkoutPlan workoutPlan)
        {
            // Logica pentru Plan: Scor bazat pe complexitate și durată
            double score = 30.0;
            
            // +5 puncte pentru fiecare săptămână
            score += workoutPlan.DurationWeeks * 2;
            
            // +1 punct pentru fiecare exercițiu
            score += workoutPlan.Exercises.Count * 1;
            
            Results.Add(new VisitorResult 
            { 
                EntityName = $"Plan: {workoutPlan.Name}", 
                Score = Math.Min(score, 100),
                Category = "Plan Complexity",
                Analysis = $"Planul durează {workoutPlan.DurationWeeks} săptămâni și conține {workoutPlan.Exercises.Count} exerciții."
            });
        }
    }

    public class VisitorResult
    {
        public string EntityName { get; set; } = "";
        public double Score { get; set; }
        public string Category { get; set; } = "";
        public string Analysis { get; set; } = "";
    }
}
