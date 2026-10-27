namespace RunnerMeets.Models;

public class SupplyStation
{
    public long Id { get; set; }

    public long PlaceId { get; set; }
    public Place Place { get; set; } = null!;
    public string? StationType { get; set; }
    public string? Notes { get; set; }

}