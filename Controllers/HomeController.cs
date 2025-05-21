using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;        // Pastikan sudah pakai ClosedXML di ExportExcel
using Rotativa.AspNetCore;   // Untuk ExportPdf
using System.Collections.Generic;
using System.IO;
using ExportDemo.Models;     // namespace Product jika perlu

public class HomeController : Controller
{
    private List<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Id = 1, Name = "Product A", Price = 10 },
            new Product { Id = 2, Name = "Product B", Price = 20 },
            new Product { Id = 3, Name = "Product C", Price = 30 }
        };
    }

    public IActionResult Index()
    {
        var products = GetProducts();
        return View(products);
    }

    public IActionResult ExportExcel()
    {
        var products = GetProducts();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Products");

        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Price";

        int row = 2;
        foreach(var p in products)
        {
            worksheet.Cell(row, 1).Value = p.Id;
            worksheet.Cell(row, 2).Value = p.Name;
            worksheet.Cell(row, 3).Value = p.Price;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        string fileName = "products.xlsx";
        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        return File(stream.ToArray(), contentType, fileName);
    }

    // Tambahan action ExportPdf
    public IActionResult ExportPdf()
    {
        var products = GetProducts();

        // Asumsikan kamu sudah punya view "PdfView.cshtml" yang menerima model List<Product>
        return new ViewAsPdf("PdfView", products)
        {
            FileName = "products.pdf"
        };
    }
}
