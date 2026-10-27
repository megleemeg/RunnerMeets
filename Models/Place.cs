namespace RunnerMeets.Models;

public class Place
{
    public long Id { get; set; }

    public string? GooglePlaceId { get; set; }
    public string? Name { get; set; }
    public string? FormattedAddress { get; set; }
    public string? PlaceType { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}