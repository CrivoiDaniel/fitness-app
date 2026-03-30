using System;
using System.IO;
using ClosedXML.Excel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

class Program
{
    static void Main()
    {
        try {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Workout Plan");
            worksheet.Cell(1, 1).Value = "Test";
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            File.WriteAllBytes("test.xlsx", stream.ToArray());
            Console.WriteLine("Excel OK");
        } catch (Exception ex) {
            Console.WriteLine("Excel Error: " + ex.Message);
        }

        try {
            var _document = new PdfDocument();
            var _page = _document.AddPage();
            var _gfx = XGraphics.FromPdfPage(_page);
            var _titleFont = new XFont("Arial", 20, XFontStyle.Bold);
            _gfx.DrawString("Test", _titleFont, XBrushes.Black, new XRect(40, 40, _page.Width - 80, 40), XStringFormats.TopLeft);
            using var stream2 = new MemoryStream();
            _document.Save(stream2, false);
            File.WriteAllBytes("test.pdf", stream2.ToArray());
            Console.WriteLine("PDF OK");
            Console.WriteLine("PDF size: " + stream2.ToArray().Length);
        } catch (Exception ex) {
            Console.WriteLine("PDF Error: " + ex.Message);
        }
    }
}
