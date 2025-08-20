using System.IO;
using System;
using System.Collections.Generic;

namespace SalesSystem;

public class Snacks
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = 0.0m;
    public int Weight { get; set; } = 0;
    public int Vat { get; set; } = 0;
    
    public static void SnacksLoader() {
        List<Snacks> snacks = new List<Snacks>();
        using (StreamReader reader = new StreamReader("Snacks.txt")) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                var parts = line.Split(',');
                if (parts.Length == 4) {
                    var snack = new Snacks {
                        Name = parts[0].Trim(),
                        Price = decimal.Parse(parts[1].Trim()),
                        Weight = int.Parse(parts[2].Trim()),
                        Vat = int.Parse(parts[3].Trim())
                    };
                    snacks.Add(snack);
                }

            }
        }
        Console.WriteLine("HEJ");
        foreach (var snack in snacks)
        {
            Console.WriteLine($"Name: {snack.Name}, Price: {snack.Price:C}, Weight: {snack.Weight}g, VAT: {snack.Vat}%");
        }
    }
}
