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
            public string Title { get; set; } = "";
            public string Day { get; set; } = "";
            public string Time { get; set; } = "";
            public string Year { get; set; } = "";
            public string Rating { get; set; } = "";
            public string Duration { get; set; } = "";
            public string Description { get; set; } = "";
            public List<int> Seats { get; set; } = new List<int>();
            public int Price { get; set; }
        }

        /// <summary>
        /// Reads movie data from a text file and returns a list of MovieInfo objects.
        /// Each movie entry includes title, day, time, year, rating, duration, description, seats, and price.
        /// </summary>
        public static List<MovieInfo> ReadMovies(string filePath)
        {
            var movies = new List<MovieInfo>();
            if (!File.Exists(filePath)) return movies;
            var lines = File.ReadAllLines(filePath);
            MovieInfo? current = null;
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    if (current != null)
                    {
                        movies.Add(current);
                        current = null;
                    }
                    continue;
                }
                if (trimmed.StartsWith("day:"))
                {
                    current = new MovieInfo();
                    current.Day = trimmed.Substring(4).Trim();
                }
                else if (current != null)
                {
                    if (trimmed.StartsWith("time:"))
                        current.Time = trimmed.Substring(5).Trim();
                    else if (trimmed.StartsWith("title:"))
                        current.Title = trimmed.Substring(6).Trim();
                    else if (trimmed.StartsWith("year:"))
                        current.Year = trimmed.Substring(5).Trim();
                    else if (trimmed.StartsWith("rating:"))
                        current.Rating = trimmed.Substring(7).Trim();
                    else if (trimmed.StartsWith("duration:"))
                        current.Duration = trimmed.Substring(9).Trim();
                    else if (trimmed.StartsWith("description:"))
                        current.Description = trimmed.Substring(12).Trim();
                    else if (trimmed.StartsWith("seats:"))
                    {
                        var seatStr = trimmed.Substring(6).Trim();
                        current.Seats = seatStr.Split(',').Select(s => int.Parse(s.Trim())).ToList();
                    }
                }
            }
            if (current != null) movies.Add(current);
            // Set price based on time
            foreach (var m in movies)
            {
                m.Price = m.Time == "13:00" ? 105 : 130;
            }
            return movies;
        }

        /// <summary>
        /// Displays the list of movies, allows the user to select a movie and choose seats.
        /// Removes chosen seats from availability and saves the selection to summary.txt.
        /// </summary>

        public static void ShowMoviesList(List<MovieInfo> movies)
        {
            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Ignore if console clear is not supported
            }
            Console.WriteLine("==== Movies List ====");
            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                Console.WriteLine($"{i + 1}. {movie.Title} ({movie.Day} {movie.Time}) - {movie.Price} SEK");
            }

            Console.Write("Choose a movie by number: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= movies.Count)
            {
                var selectedMovie = movies[choice - 1];
                Console.Clear();
                Console.WriteLine($"You chose: {selectedMovie.Title} ({selectedMovie.Day} {selectedMovie.Time})");
                Console.WriteLine($"Year: {selectedMovie.Year}, Rating: {selectedMovie.Rating}, Duration: {selectedMovie.Duration}");
                Console.WriteLine($"Description: {selectedMovie.Description}");
                Console.WriteLine($"Price: {selectedMovie.Price} SEK");
                Console.WriteLine("Available seats:");
                foreach (var seat in selectedMovie.Seats)
                {
                    Console.Write(seat + " ");
                }
                Console.WriteLine();
                Console.Write("Choose seat(s) (comma separated, e.g. 1,2,3): ");
                string? seatInput = Console.ReadLine();
                var chosenSeats = new List<int>();
                if (!string.IsNullOrWhiteSpace(seatInput))
                {
                    var seatParts = seatInput.Split(',');
                    foreach (var part in seatParts)
                    {
                        if (int.TryParse(part.Trim(), out int seatNum) && selectedMovie.Seats.Contains(seatNum))
                        {
                            chosenSeats.Add(seatNum);
                        }
                    }
                }
                if (chosenSeats.Count > 0)
                {
                    Console.WriteLine($"You chose seat(s): {string.Join(", ", chosenSeats)}");
                    // Remove chosen seats from available seats
                    foreach (var seat in chosenSeats)
                    {
                        selectedMovie.Seats.Remove(seat);
                    }
                    // Save selection to summary.txt in project root
                    string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
                    string summaryFilePath = Path.Combine(projectRoot, "summary.txt");
                    using (StreamWriter sw = new StreamWriter(summaryFilePath, append: true))
                    {
                        sw.WriteLine($"Movies,{selectedMovie.Title},{selectedMovie.Price}");
                    }

                }
                else
                {
                    Console.WriteLine("No valid seats chosen.");
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
