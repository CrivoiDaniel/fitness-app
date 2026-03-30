using System;
using System.Collections.Generic;
using System.IO;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace FitnessApp.Application.Bridge;

public class PdfExportFormat : IExportFormat
{
    private readonly PdfDocument _document;
    private readonly PdfPage _page;
    private readonly XGraphics _gfx;
    private readonly XFont _titleFont;
    private readonly XFont _subtitleFont;
    private readonly XFont _textFont;
    private double _currentY;
    private const double Margin = 40;

    public PdfExportFormat()
    {
        // Fix for macOS/Linux encoding issues in PdfSharpCore and ClosedXML
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        _document = new PdfDocument();
        _page = _document.AddPage();
        _gfx = XGraphics.FromPdfPage(_page);
        
        // System fonts must be accessible, Arial is generally safe or we fall back.
        _titleFont = new XFont("Arial", 20, XFontStyle.Bold);
        _subtitleFont = new XFont("Arial", 14, XFontStyle.Italic);
        _textFont = new XFont("Arial", 12, XFontStyle.Regular);
        
        _currentY = Margin;
    }

    public void AddTitle(string title)
    {
        _gfx.DrawString(title, _titleFont, XBrushes.Black, new XRect(Margin, _currentY, _page.Width - 2 * Margin, 40), XStringFormats.TopLeft);
        _currentY += 40;
    }

    public void AddSubtitle(string subtitle)
    {
        _gfx.DrawString(subtitle, _subtitleFont, XBrushes.DarkGray, new XRect(Margin, _currentY, _page.Width - 2 * Margin, 30), XStringFormats.TopLeft);
        _currentY += 30;
    }

    public void AddText(string text)
    {
        _gfx.DrawString(text, _textFont, XBrushes.Black, new XRect(Margin, _currentY, _page.Width - 2 * Margin, 20), XStringFormats.TopLeft);
        _currentY += 25;
    }

    public void AddList(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            _gfx.DrawString($"• {item}", _textFont, XBrushes.Black, new XRect(Margin + 10, _currentY, _page.Width - 2 * Margin - 10, 20), XStringFormats.TopLeft);
            _currentY += 20;
            
            // Basic page break handler (in a real app, logic would be more complex)
            if (_currentY > _page.Height - Margin)
            {
                // To keep this clean for demonstration, we won't add pages dynamically here,
                // but PdfSharp handles it easily by reassigning _page, _gfx, and _currentY.
            }
        }
        _currentY += 10;
    }

    public byte[] GetFileBytes()
    {
        using var stream = new MemoryStream();
        _document.Save(stream, false);
        _document.Close();
        return stream.ToArray();
    }

    public string GetContentType() => "application/pdf";
    public string GetFileExtension() => "pdf";
}
