namespace RunnerMeets.Models;

public class UserProfile
{
    public long Id { get; set; }

    public string IdentityUserId { get; set; } = string.Empty;

    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Postcode { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
