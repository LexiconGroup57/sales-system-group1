using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SalesSystem.Models;

namespace SalesSystem.Services;

internal class SeatsService
{
    public const int Rows = 6;
    public const int Cols = 9;
    public const int TotalSeats = Rows * Cols; // 54

    private static string SeatsDir => Path.Combine(DataPaths.BaseDir, "Seats");

    public string GetSessionKey(Session s)
    {
        // Build a safe key based on title + day + time
        string raw = $"{s.Title}_{s.Day}_{s.Time}";
        foreach (var c in Path.GetInvalidFileNameChars()) raw = raw.Replace(c, '_');
        return new string(raw.Select(ch => char.IsWhiteSpace(ch) ? '_' : ch).ToArray());
    }

    public string GetSeatFilePath(Session s)
    {
        Directory.CreateDirectory(SeatsDir);
        return Path.Combine(SeatsDir, $"seats_{GetSessionKey(s)}.txt");
    }

    public HashSet<int> LoadBookedSeats(string path)
    {
        var booked = new HashSet<int>();
        if (!File.Exists(path)) return booked;
        var content = File.ReadAllText(path, Encoding.UTF8).Trim();
        if (string.IsNullOrWhiteSpace(content)) return booked;
        foreach (var part in content.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (int.TryParse(part, out var n) && n >= 1 && n <= TotalSeats)
                booked.Add(n);
        }
        return booked;
    }

    public void SaveBookedSeats(string path, HashSet<int> booked)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var line = string.Join(",", booked.OrderBy(x => x));
        File.WriteAllText(path, line, Encoding.UTF8);
    }

    public void PrintSeatMap(HashSet<int> booked)
    {
        Console.WriteLine("\n==== Seat Map (X = booked) ====");
        int n = 1;
        for (int r = 0; r < Rows; r++)
        {
            var row = new StringBuilder();
            for (int c = 0; c < Cols; c++)
            {
                bool isBooked = booked.Contains(n);
                string cell = isBooked ? " X " : n.ToString().PadLeft(2) + " ";
                row.Append(cell);
                n++;
            }
            Console.WriteLine(row.ToString().TrimEnd());
        }
        Console.WriteLine();
    }

    public bool TryParseSelection(string? input, int expectedCount, HashSet<int> booked, out List<int> selection, out string? error)
    {
        selection = new List<int>();
        error = null;
        if (string.IsNullOrWhiteSpace(input)) { error = "Empty input"; return false; }
        var parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var part in parts)
        {
            if (!int.TryParse(part, out int seat) || seat < 1 || seat > TotalSeats)
            {
                error = $"Invalid seat number: {part}"; return false;
            }
            if (booked.Contains(seat)) { error = $"Seat {seat} is already booked"; return false; }
            selection.Add(seat);
        }
        if (selection.Count != expectedCount)
        {
            error = $"You must select exactly {expectedCount} seat(s)"; return false;
        }
        if (selection.Distinct().Count() != selection.Count)
        {
            error = "Duplicate seat numbers"; return false;
        }
        return true;
    }
}
