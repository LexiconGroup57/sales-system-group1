using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesSystem;

namespace SalesSystem
{
    internal class Menu
    {
        public void Show()
        {
            // Load movies once for the session
            var movies = Movies.ReadMovies("movies.txt");
            while (true)
            {
                try
                {
                    Console.Clear();
                }
                catch (IOException)
                {
                    // Ignore if console clear is not supported
                }

                Console.WriteLine("==== Main Menu ====");
                Console.WriteLine("1. Movies");
                Console.WriteLine("2. Snacks");
                Console.WriteLine("3. Summary");
                Console.WriteLine("4. Pay");
                Console.WriteLine("5. Sales Log");

                Console.Write("Choose an option: ");

                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                string summaryFilePath = Path.Combine(projectRoot, "summary.txt");
                string salesLogFilePath = Path.Combine(projectRoot, "sales_log.txt");

                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Movies.ShowMoviesList(movies);
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;
                    case "2":
                        Console.WriteLine("Snacks option selected.");
                        Snacks.SnacksLoader();
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;
                    case "3":
                        Console.WriteLine("Summary option selected.");
                        Summary.ShowSummary(summaryFilePath);
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;
                    case "4":
                        Payment.SaveSummaryToSalesLog(summaryFilePath, salesLogFilePath);
                        Payment.ClearSummary(summaryFilePath);
                        Console.WriteLine("Payment complete. Summary cleared.");
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
                        break;
                        case "5":
                            var salesLog = new SalesLog(salesLogFilePath);
                            salesLog.ShowLog();
                            Console.WriteLine("Press any key to return to menu...");
                            Console.ReadKey();
                            return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
