namespace RunnerMeets.Models;

public class Event
{
    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long PlaceId { get; set; }
    public Place Place { get; set; } = null!;
    public long CreatedByUserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }

    public string? RoutePolyline { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }

    public int MinPaceSecondsPerKm { get; set; }
    public int MaxPaceSecondsPerKm { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Visibility { get; set; } = string.Empty;
    public string? Location { get; set; }
    public decimal? DistanceKm { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
