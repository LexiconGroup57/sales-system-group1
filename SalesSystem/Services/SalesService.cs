using System;
using System.Globalization;
using System.IO;
using System.Text;
using SalesSystem.Models;

namespace SalesSystem.Services;

internal class SalesService
{
    public void EnsureLogFileExists()
    {
        var path = DataPaths.SalesLogFile;
        if (!File.Exists(path))
        {
            using var _ = File.Create(path);
        }
    }

    public void LogSale(SaleRecord sale)
    {
        EnsureLogFileExists();
        // CSV line: timestamp|type|name|qty|unitPrice|vat%|gross|vatAmount|net|notes
        var line = string.Join("|", new string[]
        {
            sale.Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
            sale.ItemType.ToString(),
            Escape(sale.ItemName),
            sale.Quantity.ToString(CultureInfo.InvariantCulture),
            sale.UnitPrice.ToString(CultureInfo.InvariantCulture),
            sale.VatPercent.ToString(CultureInfo.InvariantCulture),
            sale.Gross.ToString(CultureInfo.InvariantCulture),
            sale.VatAmount.ToString(CultureInfo.InvariantCulture),
            sale.Net.ToString(CultureInfo.InvariantCulture),
            Escape(sale.Notes ?? string.Empty)
        });
        using var sw = new StreamWriter(DataPaths.SalesLogFile, append: true, Encoding.UTF8);
        sw.WriteLine(line);
    }

    private static string Escape(string input) => input.Replace("|", "/");
}
