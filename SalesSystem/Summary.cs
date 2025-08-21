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
                  if (parts.Length == 4)
                  {
                     // Snacks: Name, Price, Weight, VAT
                     Console.WriteLine($"Snack: {parts[0]}, Price: {parts[1]} SEK, Weight: {parts[2]}g, VAT: {parts[3]}%");
                  }
                  else if (parts.Length == 2)
                  {
                     // Movie: Name, Price
                     Console.WriteLine($"Movie: {parts[0]}, Price: {parts[1]} SEK");
                  }
                  else if (line.StartsWith("Movie:"))
                  {
                     // Old format, just print
                     Console.WriteLine(line);
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
