using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Application.Bridge;

public abstract class WorkoutExporter
{
    // BRIDGE-ul către ierarhia de implementare
    protected readonly IExportFormat _exportFormat;
    protected WorkoutExporter(IExportFormat exportFormat)
    {
        _exportFormat = exportFormat ?? throw new ArgumentNullException(nameof(exportFormat));
    }
    // Metoda pe care derivatele o vor implementa cu logica lor specifică de business
    public abstract byte[] GenerateExport(WorkoutPlan workout);
    public string GetContentType() => _exportFormat.GetContentType();
    public string GetFileName(WorkoutPlan workout) 
        => $"{workout.Name.Replace(" ", "_")}.{_exportFormat.GetFileExtension()}";
}