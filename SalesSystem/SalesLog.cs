using System;
using System.Collections.Generic;
using System.IO;

namespace SalesSystem
{
    internal class SalesLog
    {
        public List<string> Movies { get; private set; } = new List<string>();
        public List<string> Snacks { get; private set; } = new List<string>();

        public SalesLog(string filePath)
        {
            Load(filePath);
        }

        private void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;
            using (var reader = new StreamReader(filePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length < 4) continue;
                    var type = parts[1].Trim();
                    if (type.Equals("Movies", StringComparison.OrdinalIgnoreCase))
                    {
                        Movies.Add(line);
                    }
                    else if (type.Equals("Snacks", StringComparison.OrdinalIgnoreCase))
                    {
                        Snacks.Add(line);
                    }
                }
            }
        }

        private void CountMoviesVat()
        {
            // VAT is 25% for movies
            var movieSales = Movies.Sum(m => int.Parse(m.Split(',')[3].Trim()));
            var totalSales = movieSales;
            var vat = totalSales * 0.25m;
            Console.WriteLine($"Total VAT for movies: {vat:C}");
        }

        private void CountSnackVat()
        {
            // VAT is 12% for snacks
            var snackSales = Snacks.Sum(s => int.Parse(s.Split(',')[3].Trim()));
            var totalSales = snackSales;
            var vat = totalSales * 0.12m;
            Console.WriteLine($"Total VAT for snacks: {vat:C}");
        }

        public void ShowLog()
        {
            Console.WriteLine("==== Sales Log ====");
            Console.WriteLine("-- Movies --");
            if (Movies.Count == 0) Console.WriteLine("No movie sales.");
            Console.WriteLine(Movies.Count());
            CountMoviesVat();

            Console.WriteLine("-- Snacks --");
            if (Snacks.Count == 0) Console.WriteLine("No snack sales.");
            Console.WriteLine(Snacks.Count());
            CountSnackVat();
        }
    }
}
