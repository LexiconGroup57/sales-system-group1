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
            public string Day { get; set; } = "";
            public string Time { get; set; } = "";
            public int Price { get; set; }
        }

        public static List<MovieInfo> ReadMovies(string filePath)
        {
            var movies = new List<MovieInfo>();
            if (!File.Exists(filePath)) return movies;
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;
                // Find showtime line (contains comma)
                if (line.Contains(","))
                {
                    var showtimeParts = line.Split(',')[0].Trim().Split(' ');
                    if (showtimeParts.Length == 2)
                    {
                        string day = showtimeParts[0];
                        string time = showtimeParts[1];
                        int price = time == "13:00" ? 105 : 130;
                        // Next line is movie name
                        if (i + 1 < lines.Length)
                        {
                            string name = lines[i + 1].Trim();
                            movies.Add(new MovieInfo
                            {
                                Name = name,
                                Day = day,
                                Time = time,
                                Price = price
                            });
                        }
                    }
                }
            }
            return movies;
        }

        public static void ShowMoviesList(string filePath)
        {
            var movies = ReadMovies(filePath);
            Console.Clear();
            Console.WriteLine("==== Movies List ====");
            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                Console.WriteLine($"{i + 1}. {movie.Name} ({movie.Day} {movie.Time}) - {movie.Price} SEK");
            }

            Console.Write("Choose a movie by number: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= movies.Count)
            {
                var selectedMovie = movies[choice - 1];
                Console.WriteLine($"You chose: {selectedMovie.Name} - {selectedMovie.Price} SEK");
                // Save selection to summary.txt in project root
                string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                string summaryFilePath = Path.Combine(projectRoot, "summary.txt");
                using (StreamWriter sw = new StreamWriter(summaryFilePath, append: true))
                {
                    sw.WriteLine($"Movies,{selectedMovie.Name},{selectedMovie.Price}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
            Console.WriteLine("Press any key to return to menu...");
            Console.ReadKey();
        }
    }
}
