using System.Text;

namespace FitnessApp.Application.Bridge;

public class HtmlExportFormat : IExportFormat
{
    private readonly StringBuilder _sb = new();
    public HtmlExportFormat()
    {
        _sb.AppendLine("<!DOCTYPE html><html><head><meta charset=\"UTF-8\"></head><body style=\"font-family: Arial, sans-serif;\">");
    }
    public void AddTitle(string title) => _sb.AppendLine($"<h1 style=\"color:#2C3E50;\">{title}</h1>");
    public void AddSubtitle(string subtitle) => _sb.AppendLine($"<h3 style=\"color:#2980B9;\">{subtitle}</h3>");
    public void AddText(string text) => _sb.AppendLine($"<p>{text}</p>");
    
    public void AddList(IEnumerable<string> items)
    {
        _sb.AppendLine("<ul>");
        foreach (var item in items)
        {
            _sb.AppendLine($"<li>{item}</li>");
        }
        _sb.AppendLine("</ul>");
    }
    public byte[] GetFileBytes()
    {
        _sb.AppendLine("</body></html>");
        return Encoding.UTF8.GetBytes(_sb.ToString());
    }
    public string GetContentType() => "text/html";
    public string GetFileExtension() => "html";
}