namespace RunnerMeets.Models;

public class RunnerProfile
{
    public long Id { get; set; }

    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public string? Bio { get; set; }
    public string? ExperienceLevel { get; set; }
    public int? MinPaceSecondsPerKm { get; set; }
    public int? MaxPaceSecondsPerKm { get; set; }
    public string? TrainingFor { get; set; }
    public decimal? PreferredDistanceKm { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}