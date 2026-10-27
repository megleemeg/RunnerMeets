namespace RunnerMeets.Models;

public class EventToRegistration
{
    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public long UserId { get; set; }
    public UserProfile User { get; set; } = null!;

    public string? RegistrationStatus { get; set; }

    public DateTime JoinedAt { get; set; }
    public DateTime CancelledAt { get; set; }
    
}