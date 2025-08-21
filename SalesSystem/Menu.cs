using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem
{
    internal class Menu
    {
        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("==== Main Menu ====");
                Console.WriteLine("1. Movies - Sell ticket");
                Console.WriteLine("2. Snacks - Sell snack");
                Console.WriteLine("3. Monthly Summary");
                Console.WriteLine("4. Quit");
                Console.WriteLine();

                Console.Write("Choose an option: ");

                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        HandleTicketSale();
                        break;
                    case "2":
                        HandleSnackSale();
                        break;
                    case "3":
                        ShowMonthlySummary();
                        break;
                    case "4":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void HandleTicketSale()
        {
            var movieService = new Services.MovieService();
            var sales = new Services.SalesService();
            var sessions = movieService.LoadSessions(Services.DataPaths.MoviesFile);
            if (sessions.Count == 0)
            {
                Console.WriteLine("No sessions available. Ensure movies.txt is present.");
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                return;
            }
            Console.Clear();
            Console.WriteLine("==== Available Sessions ====");
            for (int i = 0; i < sessions.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sessions[i]}");
            }
            Console.Write("Choose a session by number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > sessions.Count)
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                return;
            }
            var selected = sessions[choice - 1];
            Console.WriteLine("\n==== Ticket Prices ====");
            Console.WriteLine("Regular (18:00, 21:00): 130 SEK");
            Console.WriteLine("Regular (13:00):        105 SEK");
            Console.WriteLine("Child < 6:             Free (with paying customer)");
            Console.WriteLine("Child 6–11:             65 SEK");
            Console.WriteLine("Senior 67+:             90 SEK");

            int Count(string prompt)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                return int.TryParse(s, out var n) && n >= 0 ? n : -1;
            }

            Console.WriteLine();
            int qtyUnder6 = Count("Number of children under 6 (free): ");
            if (qtyUnder6 < 0) { Console.WriteLine("Invalid number."); Console.ReadKey(); return; }
            int qty6to11 = Count("Number of children 6–11: ");
            if (qty6to11 < 0) { Console.WriteLine("Invalid number."); Console.ReadKey(); return; }
            int qty67plus = Count("Number of seniors 67+: ");
            if (qty67plus < 0) { Console.WriteLine("Invalid number."); Console.ReadKey(); return; }
            int qtyRegular = Count("Number of regular tickets: ");
            if (qtyRegular < 0) { Console.WriteLine("Invalid number."); Console.ReadKey(); return; }

            int payingCount = qty6to11 + qty67plus + qtyRegular;
            int totalCount = qtyUnder6 + payingCount;
            if (totalCount <= 0)
            {
                Console.WriteLine("No tickets selected.");
                Console.ReadKey();
                return;
            }
            if (qtyUnder6 > 0 && payingCount == 0)
            {
                Console.WriteLine("Children under 6 must be accompanied by a paying customer.");
                Console.ReadKey();
                return;
            }

            // Seat selection
            var seatsSvc = new Services.SeatsService();
            var seatFile = seatsSvc.GetSeatFilePath(selected);
            var booked = seatsSvc.LoadBookedSeats(seatFile);
            seatsSvc.PrintSeatMap(booked);
            Console.Write($"Select {totalCount} seat number(s), comma-separated: ");
            var inputSeats = Console.ReadLine();
            if (!seatsSvc.TryParseSelection(inputSeats, totalCount, booked, out var chosen, out var err))
            {
                Console.WriteLine(err);
                Console.ReadKey();
                return;
            }
            foreach (var s in chosen) booked.Add(s);
            seatsSvc.SaveBookedSeats(seatFile, booked);

            // VAT rates per spec: Tickets 25%
            const int ticketVat = 25;

            // Determine regular unit price by time
            int regularPrice = selected.PriceSek; // already 105 or 130
            int price6to11 = 65;
            int price67plus = 90;
            int priceUnder6 = 0;

            decimal totalGross = 0m;
            decimal totalVat = 0m;

            void LogIfAny(int qty, int unitPrice, string label)
            {
                if (qty <= 0) return;
                var rec = new Models.SaleRecord
                {
                    Timestamp = DateTime.Now,
                    ItemType = Models.SaleItemType.Ticket,
                    ItemName = $"{selected.Title} {selected.Day} {selected.Time} ({label})",
                    Quantity = qty,
                    UnitPrice = unitPrice,
                    VatPercent = ticketVat,
                    Notes = selected.YearRatingDuration
                };
                sales.LogSale(rec);
                totalGross += rec.Gross;
                totalVat += rec.VatAmount;
            }

            LogIfAny(qtyRegular, regularPrice, "Regular");
            LogIfAny(qty6to11, price6to11, "Child 6–11");
            LogIfAny(qty67plus, price67plus, "Senior 67+");
            LogIfAny(qtyUnder6, priceUnder6, "Child <6");

            Console.WriteLine($"\nSold {totalCount} ticket(s) for {selected.Title}.");
            Console.WriteLine($"Seats: {string.Join(", ", chosen.OrderBy(x => x))}");
            Console.WriteLine($"Total: {totalGross} SEK (incl. VAT {totalVat} SEK)");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private void HandleSnackSale()
        {
            var snackService = new Services.SnackService();
            var sales = new Services.SalesService();
            var snacks = snackService.LoadSnacks(Services.DataPaths.SnacksFile);
            if (snacks.Count == 0)
            {
                Console.WriteLine("No snacks available. Ensure Snacks.txt is present.");
                Console.WriteLine("Press any key to return to menu...");
                Console.ReadKey();
                return;
            }
            Console.Clear();
            Console.WriteLine("==== Snacks Menu ====");
            for (int i = 0; i < snacks.Count; i++)
            {
                var s = snacks[i];
                Console.WriteLine($"{i + 1}. {s.Name} - {s.Price} SEK ({s.WeightGrams}g, VAT {s.VatPercent}%)");
            }
            Console.Write("Choose a snack by number: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > snacks.Count)
            {
                Console.WriteLine("Invalid choice.");
                Console.ReadKey();
                return;
            }
            var selected = snacks[choice - 1];
            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 1)
            {
                Console.WriteLine("Invalid quantity.");
                Console.ReadKey();
                return;
            }
            var sale = new Models.SaleRecord
            {
                Timestamp = DateTime.Now,
                ItemType = Models.SaleItemType.Snack,
                ItemName = selected.Name,
                Quantity = qty,
                UnitPrice = selected.Price,
                VatPercent = selected.VatPercent,
                Notes = $"{selected.WeightGrams}g"
            };
            sales.LogSale(sale);
            Console.WriteLine($"Sold {qty} x {selected.Name}. Total: {sale.Gross} SEK (incl. VAT {sale.VatAmount} SEK)");
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }

        private void ShowMonthlySummary()
        {
            // Year is fixed to 2025 by default (no prompt)
            const int year = 2025;
            Console.Write("Enter month (1-12), leave empty for current: ");
            var mIn = Console.ReadLine();
            var now = DateTime.Now;
            int month = string.IsNullOrWhiteSpace(mIn) ? now.Month : int.TryParse(mIn, out var m) ? m : now.Month;

            var report = new Services.ReportService();
            var sum = report.GetMonthlySummary(year, month);
            Console.WriteLine($"\n==== Summary for {year}-{month:00} ====");
            Console.WriteLine($"Total Gross: {sum.TotalGross} SEK");
            Console.WriteLine($"Total VAT:   {sum.TotalVat} SEK");
            Console.WriteLine($"Total Net:   {sum.TotalNet} SEK\n");

            decimal tickets = sum.GrossByType.TryGetValue("Ticket", out var tg) ? tg : 0m;
            decimal snacks = sum.GrossByType.TryGetValue("Snack", out var sg) ? sg : 0m;
            Console.WriteLine($"Total tickets sale: {tickets} SEK");
            Console.WriteLine($"Total snacks sale:  {snacks} SEK\n");

            decimal vat25 = sum.VatByRate.TryGetValue(25, out var v25) ? v25 : 0m;
            decimal vat12 = sum.VatByRate.TryGetValue(12, out var v12) ? v12 : 0m;
            Console.WriteLine($"25% VAT: {vat25} SEK");
            Console.WriteLine($"12% VAT: {vat12} SEK");

            // If you want to display other VAT rates present
            foreach (var kv in sum.VatByRate.Where(k => k.Key != 25 && k.Key != 12))
            {
                Console.WriteLine($"{kv.Key}% VAT: {kv.Value} SEK");
            }
            Console.WriteLine("\nPress any key to return to menu...");
            Console.ReadKey();
        }
    }
}
