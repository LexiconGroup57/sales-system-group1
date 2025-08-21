using System;
using System.IO;

namespace SalesSystem.Services;

internal static class DataPaths
{
    public static string BaseDir => AppContext.BaseDirectory;
    public static string MoviesFile => Path.Combine(BaseDir, "movies.txt");
    public static string SnacksFile => Path.Combine(BaseDir, "Snacks.txt");
    public static string SalesLogFile => Path.Combine(BaseDir, "sales_log.txt");
}
