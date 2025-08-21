using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SalesSystem.Models;

namespace SalesSystem.Services;

internal class MovieService
{
    // Parse movies.txt into a list of sessions using StreamReader
    public List<Session> LoadSessions(string filePath)
    {
        var sessions = new List<Session>();
        if (!File.Exists(filePath)) return sessions;
        using var sr = new StreamReader(filePath, Encoding.UTF8);
        string? line;
        while ((line = sr.ReadLine()) != null)
        {
            line = line.Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            // First line: Day Time[,]
            var first = line.TrimEnd(',');
            var parts = first.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) continue;
            string day = parts[0];
            string time = parts[1];

            string? title = sr.ReadLine()?.Trim();
            string? yrd = sr.ReadLine()?.Trim();

            // Read description lines until blank line or EOF
            var descSb = new StringBuilder();
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) break;
                descSb.AppendLine(line.Trim());
            }

            // Pricing rule: 13:00 -> 105 SEK else 130 SEK
            int price = time == "13:00" ? 105 : 130;

            sessions.Add(new Session
            {
                Day = day,
                Time = time,
                Title = title ?? string.Empty,
                YearRatingDuration = yrd,
                Description = descSb.ToString().TrimEnd(),
                PriceSek = price
            });
        }
        return sessions;
    }
}
