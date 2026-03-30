using System.Text;
using System.Text.Json;

namespace FitnessApp.Application.Bridge;

public class JsonExportFormat : IExportFormat
{
    private readonly Dictionary<string, object> _document = new();
    private readonly List<string> _currentList = new();
    public void AddTitle(string title) => _document["Title"] = title;
    public void AddSubtitle(string subtitle) => _document["Subtitle"] = subtitle;
    public void AddText(string text) => _document["Description"] = text;
    
    public void AddList(IEnumerable<string> items) 
    {
        _currentList.AddRange(items);
        _document["Items"] = _currentList;
    }
    public byte[] GetFileBytes()
    {
        var json = JsonSerializer.Serialize(_document, new JsonSerializerOptions { WriteIndented = true });
        return Encoding.UTF8.GetBytes(json);
    }
    public string GetContentType() => "application/json";
    public string GetFileExtension() => "json";
}