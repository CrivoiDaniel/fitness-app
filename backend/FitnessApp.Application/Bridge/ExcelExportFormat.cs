using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;

namespace FitnessApp.Application.Bridge;

public class ExcelExportFormat : IExportFormat
{
    private readonly XLWorkbook _workbook;
    private readonly IXLWorksheet _worksheet;
    private int _currentRow = 1;

    public ExcelExportFormat()
    {
        // Fix for macOS/Linux encoding issues in ClosedXML
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        _workbook = new XLWorkbook();
        _worksheet = _workbook.Worksheets.Add("Workout Plan");
    }

    public void AddTitle(string title)
    {
        var cell = _worksheet.Cell(_currentRow, 1);
        cell.Value = title;
        cell.Style.Font.Bold = true;
        cell.Style.Font.FontSize = 16;
        _currentRow += 2; // Leave a blank line
    }

    public void AddSubtitle(string subtitle)
    {
        var cell = _worksheet.Cell(_currentRow, 1);
        cell.Value = subtitle;
        cell.Style.Font.Italic = true;
        cell.Style.Font.FontSize = 12;
        cell.Style.Font.FontColor = XLColor.DimGray;
        _currentRow += 2;
    }

    public void AddText(string text)
    {
        var cell = _worksheet.Cell(_currentRow, 1);
        cell.Value = text;
        _currentRow++;
    }

    public void AddList(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            _worksheet.Cell(_currentRow, 2).Value = "• " + item;
            _currentRow++;
        }
        _currentRow++;
    }

    public byte[] GetFileBytes()
    {
        _worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        _workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public string GetContentType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    
    public string GetFileExtension() => "xlsx";
}
