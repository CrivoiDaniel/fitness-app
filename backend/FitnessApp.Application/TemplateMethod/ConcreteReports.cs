using System.Collections.Generic;

namespace FitnessApp.Application.TemplateMethod
{
    // Raport pentru Scădere în Greutate
    public class WeightLossReportGenerator : ProgressReportGenerator
    {
        protected override Dictionary<string, string> CalculateStatistics(string rawData)
        {
            return new Dictionary<string, string>
            {
                { "Greutate Pierdută", "4.5 kg" },
                { "Calorii Medii/Zi", "1850 kcal" },
                { "Deficit caloric", "500 kcal" }
            };
        }

        protected override List<string> AddRecommendations(Dictionary<string, string> stats)
        {
            return new List<string>
            {
                "Crește consumul de apă cu 500ml.",
                "Menține ritmul actual de cardio.",
                "Adaugă mai multe proteine la micul dejun."
            };
        }

        protected override string FormatTitle() => "📊 Raport Evoluție Greutate (Weight Loss)";
    }

    // Raport pentru Forță și Masă Musculară
    public class StrengthReportGenerator : ProgressReportGenerator
    {
        protected override Dictionary<string, string> CalculateStatistics(string rawData)
        {
            return new Dictionary<string, string>
            {
                { "Record Bench Press", "100 kg (+5kg)" },
                { "Volum Total Săptămânal", "15,000 kg" },
                { "Intensitate Medie", "85%" }
            };
        }

        protected override List<string> AddRecommendations(Dictionary<string, string> stats)
        {
            return new List<string>
            {
                "Focalizează-te pe tehnica la Deadlift.",
                "Mărește timpul de odihnă între seturile grele la 3 minute.",
                "Încearcă o periodizare nouă luna viitoare."
            };
        }

        protected override string FormatTitle() => "💪 Raport Performanță și Forță (Strength)";
    }
}
