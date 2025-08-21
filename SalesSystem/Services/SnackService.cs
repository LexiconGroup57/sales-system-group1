using System.Collections.Generic;
using System.IO;
using System.Text;
using SalesSystem.Models;

namespace SalesSystem.Services;

internal class SnackService
{
    public List<Snack> LoadSnacks(string filePath)
    {
        var snacks = new List<Snack>();
        if (!File.Exists(filePath)) return snacks;
        using var sr = new StreamReader(filePath, Encoding.UTF8);
        string? line;
        string? name = null; int weight = 0; decimal price = 0; int vat = 0;
        while ((line = sr.ReadLine()) != null)
        {
            var t = line.Trim();
            if (string.IsNullOrEmpty(t)) continue;
            if (t.StartsWith("Item:"))
            {
                name = t.Substring(5).Trim();
            }
            else if (t.StartsWith("Weight:"))
            {
                var w = t.Substring(7).Trim().TrimEnd('g', 'G');
                int.TryParse(w, out weight);
            }
            else if (t.StartsWith("Price:"))
            {
                var p = t.Substring(6).Trim();
                decimal.TryParse(p, out price);
            }
            else if (t.StartsWith("Vat:"))
            {
                var v = t.Substring(4).Trim().TrimEnd('%');
                int.TryParse(v, out vat);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    snacks.Add(new Snack { Name = name!, WeightGrams = weight, Price = price, VatPercent = vat });
                    name = null; weight = 0; price = 0; vat = 0;
                }
            }
        }
        return snacks;
    }
}
