namespace SalesSystem.Models;

public class Session
{
    public string Day { get; set; } = ""; // e.g., Tuesday
    public string Time { get; set; } = ""; // e.g., 18:00
    public string Title { get; set; } = "";
    public string? YearRatingDuration { get; set; }
    public string? Description { get; set; }
    public int PriceSek { get; set; }
    public override string ToString() => $"{Day} {Time} - {Title} ({YearRatingDuration}) {PriceSek} SEK";
}
