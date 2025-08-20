using System;
using System.Collections.Generic;
using System.IO;

namespace SalesSystem;

public class Snacks
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; } = 0.0m;
    public int Weight { get; set; } = 0;
    public int Vat { get; set; } = 0;

    public static void SnacksLoader()
    {
        List<Snacks> snacks = new List<Snacks>();
        using (StreamReader reader = new StreamReader("Snacks.txt"))
        {
            string? name = null;
            int weight = 0;
            decimal price = 0;
            int vat = 0;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("Item:"))
                {
                    name = line.Substring(5).Trim();
                }
                else if (line.StartsWith("Weight:"))
                {
                    var w = line.Substring(7).Trim().Replace("g", "");
                    int.TryParse(w, out weight);
                }
                else if (line.StartsWith("Price:"))
                {
                    var p = line.Substring(6).Trim();
                    decimal.TryParse(p, out price);
                }
                else if (line.StartsWith("Vat:"))
                {
                    var v = line.Substring(4).Trim().Replace("%", "");
                    int.TryParse(v, out vat);
                    // Add snack when all fields are read
                    if (!string.IsNullOrEmpty(name))
                    {
                        snacks.Add(new Snacks { Name = name, Weight = weight, Price = price, Vat = vat });
                        name = null; weight = 0; price = 0; vat = 0;
                    }
                }
            }
        }
        Console.WriteLine("==== Snacks Menu ====");
        for (int i = 0; i < snacks.Count; i++)
        {
            var snack = snacks[i];
            Console.WriteLine($"{i + 1}. Name: {snack.Name}, Price: {snack.Price:C}, Weight: {snack.Weight}g, VAT: {snack.Vat}%");
        }
        Console.Write("Choose a snack by number: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= snacks.Count)
        {
            var selected = snacks[choice - 1];
            Console.WriteLine($"You chose: {selected.Name} - {selected.Price:C}");
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
