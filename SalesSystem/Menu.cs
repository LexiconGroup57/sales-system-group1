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
                Console.WriteLine("1. Movies");
                Console.WriteLine("2. Snacks");
                Console.WriteLine("3. Summary");
                Console.WriteLine("4. Quit");
                Console.WriteLine("5. Hej");

                Console.Write("Choose an option: ");

                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Movie option selected. (To be implemented)");
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
                        Console.WriteLine("Summary option selected. (To be implemented)");
                        Console.WriteLine("Press any key to return to menu...");
                        Console.ReadKey();
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
    }
}
