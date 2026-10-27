namespace RunnerMeets.Models;

public class EventToSupplyStation
{
    public long EventId { get; set; }
    public Event Event { get; set; } = null!;

    public long SupplyStationId { get; set; }
    public SupplyStation SupplyStation { get; set; } = null!;
}