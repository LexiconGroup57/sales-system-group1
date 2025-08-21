using System;
using System.IO;

namespace SalesSystem
{
   internal class Summary
   {
      public static void ShowSummary(string summaryFilePath)
      {
         if (File.Exists(summaryFilePath))
         {
            var lines = File.ReadAllLines(summaryFilePath);
            if (lines.Length == 0)
            {
               Console.WriteLine("No items have been selected yet.");
            }
            else
            {
               Console.WriteLine("Your selections:");
               foreach (var line in lines)
               {
                  var parts = line.Split(',');
                  if (parts.Length == 3)
                  {
                     



                  parts[0] = parts[0].Trim();
                     if (parts[0].Equals("Movies", StringComparison.OrdinalIgnoreCase))
                     {
                        // Movies: Name, Price
                        Console.WriteLine($"Movie: {parts[1]} Price: {parts[2]} SEK");
                     }
                     else if (parts[0].Equals("Snacks", StringComparison.OrdinalIgnoreCase))
                     {
                        // Snacks: Name, Price
                        Console.WriteLine($"Snack: {parts[1]} Price: {parts[2]} SEK");
                     }
                  }
                  
               }
            }
         }
         else
         {
            Console.WriteLine("No items have been selected yet.");
         }
      }
   }
}
