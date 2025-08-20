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

    public static void SnacksLoader()
    {
        // Adjust filepath
        string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        // Set file paths for snacks and summary files in project root
        string snacksFilePath = Path.Combine(projectRoot, "Snacks.txt");
        string summaryFilePath = Path.Combine(projectRoot, "summary.txt");

        List<Snacks> snacks = new List<Snacks>();
        using (StreamReader reader = new StreamReader(snacksFilePath))
        {
            string nameLine, weightLine, priceLine, vatLine;
            while ((nameLine = reader.ReadLine()) != null)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(nameLine))
                    continue;

                weightLine = reader.ReadLine();
                priceLine = reader.ReadLine();
                vatLine = reader.ReadLine();

                if (weightLine == null || priceLine == null || vatLine == null)
                    break; // Incomplete snack entry

                try
                {
                    var snack = new Snacks
                    {
                        Name = nameLine.Replace("Item:", "").Trim(),
                        Weight = int.Parse(weightLine.Replace("Weight:", "").Replace("g", "").Trim()),
                        Price = decimal.Parse(priceLine.Replace("Price:", "").Trim()),
                        Vat = int.Parse(vatLine.Replace("Vat:", "").Replace("%", "").Trim())
                    };
                    snacks.Add(snack);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing snack entry: {ex.Message}");
                }
            }
        }

        Console.WriteLine("Snacks Menu:");
        for (int i = 0; i < snacks.Count; i++)
        {
            var snack = snacks[i];
            Console.WriteLine($"{i + 1}. {snack.Name} - {snack.Price:C} ({snack.Weight}g, VAT: {snack.Vat}%)");
        }

        Console.Write("Choose a snack by number: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int choice) && choice >= 1 && choice <= snacks.Count)
        {
            var selectedSnack = snacks[choice - 1];
            Console.WriteLine($"You chose: {selectedSnack.Name}\nPrice: {selectedSnack.Price:C}\nWeight: {selectedSnack.Weight}g\nVAT: {selectedSnack.Vat}%");

            // Save selection to summary.txt
            using (StreamWriter sw = new StreamWriter(summaryFilePath, append: true))
            {

                sw.WriteLine($"{selectedSnack.Name},{selectedSnack.Price},{selectedSnack.Weight},{selectedSnack.Vat}");
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }
}
