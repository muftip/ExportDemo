using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Rotativa.AspNetCore;
using ExportDemo.Models;

public class ExportController : Controller
{
    private List<Product> GetProducts() => new()
    {
        new Product { Id = 1, Name = "Produk A", Price = 10000 },
        new Product { Id = 2, Name = "Produk B", Price = 15000 }
    };

    public IActionResult Index() => View(GetProducts());

    public IActionResult ExportToExcel()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Produk");
        var currentRow = 1;
        worksheet.Cell(currentRow, 1).Value = "ID";
        worksheet.Cell(currentRow, 2).Value = "Nama";
        worksheet.Cell(currentRow, 3).Value = "Harga";

        foreach (var p in GetProducts())
        {
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = p.Id;
            worksheet.Cell(currentRow, 2).Value = p.Name;
            worksheet.Cell(currentRow, 3).Value = p.Price;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "produk.xlsx");
    }

    public IActionResult ExportToPdf() => new ViewAsPdf("Index", GetProducts()) { FileName = "produk.pdf" };
}
