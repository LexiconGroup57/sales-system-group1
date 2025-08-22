using System;
using System.IO;

namespace SalesSystem
{
    internal class Payment
    {
        public static void SaveSummaryToSalesLog(string summaryFilePath, string salesLogFilePath)
        {
            if (File.Exists(summaryFilePath))
            {
                using (StreamReader reader = new StreamReader(summaryFilePath))
                using (StreamWriter writer = new StreamWriter(salesLogFilePath, append: true))
                {
                    string? line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            writer.WriteLine($"{timestamp},{line}");
                        }
                    }
                }
            }
        }

        public static void ClearSummary(string summaryFilePath)
        {
            // Overwrite the summary file with nothing
            File.WriteAllText(summaryFilePath, string.Empty);
        }
    }
}
    