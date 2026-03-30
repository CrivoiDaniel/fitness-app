namespace FitnessApp.Application.Bridge;

public interface IExportFormat
{
    void AddTitle(string title);
    void AddSubtitle(string subtitle);
    void AddText(string text);
    void AddList(IEnumerable<string> items);
    byte[] GetFileBytes();
    string GetContentType();
    string GetFileExtension();
}