using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SalesSystem
{
    internal class Movies
    {
        public class MovieInfo
        {
            public string Name { get; set; } = "";
            public List<(string Day, string Time, int Price)> Showtimes { get; set; } = new();
        }

        public List<MovieInfo> ReadMovies(string filePath)
        {
            var movies = new List<MovieInfo>();
            if (!File.Exists(filePath)) return movies;
            var lines = File.ReadAllLines(filePath);
            var current = new MovieInfo();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (!string.IsNullOrWhiteSpace(current.Name))
                        movies.Add(current);
                    current = new MovieInfo();
                    continue;
                }
                if (current.Showtimes.Count == 0)
                {
                    // First line: showtimes
                    var times = line.Split(',');
                    foreach (var t in times)
                    {
                        var parts = t.Trim().Split(' ');
                        if (parts.Length == 2)
                        {
                            string day = parts[0];
                            string time = parts[1];
                            int price = time == "13:00" ? 105 : 130;
                            current.Showtimes.Add((day, time, price));
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(current.Name))
                {
                    // Second line: movie name
                    current.Name = line.Trim();
                }
                // Ignore other lines for this simple list
            }
            if (!string.IsNullOrWhiteSpace(current.Name))
                movies.Add(current);
            return movies;
        }

        public void ShowMoviesList(string filePath)
        {
            var movies = ReadMovies(filePath);
            Console.Clear();
            Console.WriteLine("==== Movies List ====");
            foreach (var movie in movies)
            {
                Console.WriteLine($"{movie.Name}");
                foreach (var st in movie.Showtimes)
                {
                    Console.WriteLine($"  {st.Day} {st.Time} - {st.Price} SEK");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }
    }
}
