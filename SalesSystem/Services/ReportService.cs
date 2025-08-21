using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SalesSystem.Models;

namespace SalesSystem.Services;

internal class ReportService
{
    public class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalGross { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalNet { get; set; }
        public Dictionary<string, decimal> GrossByType { get; set; } = new();
        public Dictionary<int, decimal> VatByRate { get; set; } = new();
    }

    public MonthlySummary GetMonthlySummary(int year, int month)
    {
        var summary = new MonthlySummary { Year = year, Month = month };
        if (!File.Exists(DataPaths.SalesLogFile)) return summary;
        foreach (var line in File.ReadLines(DataPaths.SalesLogFile, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split('|');
            if (parts.Length < 9) continue;
            if (!DateTime.TryParseExact(parts[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var ts)) continue;
            if (ts.Year != year || ts.Month != month) continue;
            var type = parts[1];
            decimal.TryParse(parts[6], NumberStyles.Any, CultureInfo.InvariantCulture, out var gross);
            decimal.TryParse(parts[7], NumberStyles.Any, CultureInfo.InvariantCulture, out var vat);
            decimal.TryParse(parts[8], NumberStyles.Any, CultureInfo.InvariantCulture, out var net);
            int.TryParse(parts[5], NumberStyles.Any, CultureInfo.InvariantCulture, out var vatRate);
            summary.TotalGross += gross;
            summary.TotalVat += vat;
            summary.TotalNet += net;
            summary.GrossByType[type] = (summary.GrossByType.TryGetValue(type, out var g) ? g : 0) + gross;
            if (vatRate > 0)
            {
                summary.VatByRate[vatRate] = (summary.VatByRate.TryGetValue(vatRate, out var v) ? v : 0) + vat;
            }
        }
        return summary;
    }
}
