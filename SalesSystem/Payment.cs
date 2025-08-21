using System;
using System.IO;

namespace SalesSystem
{
    internal class Payment
    {
        public static void ClearSummary(string summaryFilePath)
        {
            // Overwrite the summary file with nothing
            File.WriteAllText(summaryFilePath, string.Empty);
        }
    }
}
