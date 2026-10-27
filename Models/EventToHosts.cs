namespace RunnerMeets.Models;

public class EventToHosts
{
    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public string? Role { get; set; }
    public DateTime UpdatedAt { get; set; }
}