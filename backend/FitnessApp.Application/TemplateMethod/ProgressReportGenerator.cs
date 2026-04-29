using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitnessApp.Application.TemplateMethod
{
    public abstract class ProgressReportGenerator
    {
        // Aceasta este metoda Template (Șablon)
        // Este finală (nu poate fi suprascrisă) pentru a păstra ordinea pașilor
        public async Task<ReportResult> GenerateReportAsync(int clientId)
        {
            var report = new ReportResult { ClientId = clientId, GeneratedAt = DateTime.Now };

            // Pasul 1: Colectare date (Comun)
            report.Data = await FetchClientData(clientId);

            // Pasul 2: Calcul statistici (Specific fiecărui tip de raport)
            report.Statistics = CalculateStatistics(report.Data);

            // Pasul 3: Adăugare recomandări personalizate (Specific)
            report.Recommendations = AddRecommendations(report.Statistics);

            // Pasul 4: Finalizare format (Comun)
            report.FormattedTitle = FormatTitle();

            return report;
        }

        // Pas comun: Implementat direct în clasa de bază
        protected virtual async Task<string> FetchClientData(int clientId)
        {
            // Simulare colectare date din DB
            return $"Date brute pentru clientul {clientId} din ultimele 30 de zile.";
        }

        // Pași specifici: Trebuie implementați de sub-clase
        protected abstract Dictionary<string, string> CalculateStatistics(string rawData);
        protected abstract List<string> AddRecommendations(Dictionary<string, string> stats);

        // Pas opțional (Hook): Sub-clasele îl pot schimba dacă doresc
        protected virtual string FormatTitle()
        {
            return "Raport de Progres Fitness";
        }
    }

    public class ReportResult
    {
        public int ClientId { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string FormattedTitle { get; set; } = "";
        public string Data { get; set; } = "";
        public Dictionary<string, string> Statistics { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }
}
