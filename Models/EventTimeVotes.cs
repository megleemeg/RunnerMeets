namespace RunnerMeets.Models;

public class EventTimeVotes
{
    public long EventTimeOptionId { get; set; }
    public EventTimeOptions EventTimeOptions { get; set; } = null!;

    public long UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}