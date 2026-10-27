namespace RunnerMeets.Models;

public class EventTimeOptions
{
    public long Id { get; set; }

    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public long CreatedById { get; set; }
    public UserProfile UserProfile { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}