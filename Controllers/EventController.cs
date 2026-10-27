using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMeets.Models;
using System.Security.Claims;
using RunnerMeets.Services;

namespace RunnerMeets.Controllers;


[Route("api/events")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public EventController(ApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    [HttpGet("")]
    public async Task<ActionResult<List<EventDto>>> GetEvents()
    {
        var events = await _context.Events
            .AsNoTracking()
            .ToListAsync();

        var eventDtos = events.Select(e => ToDto(e)).ToList();
        return Ok(eventDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(long id)
    {
        var evt = await _context.Events.FindAsync(id);
        if (evt is null) return NotFound();
        
        return Ok(ToDto(evt));
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(long id, UpsertEventDto request)
    {        
        var userProfile = await _currentUser.GetUserProfileAsync(User);
        if (userProfile is null) return Unauthorized();

        var evt = await _context.Events.FindAsync(id);
        if (evt is null) return NotFound();

        if (evt.CreatedByUserProfileId != userProfile.Id) return Forbid();

        evt.Title = request.Title;
        evt.Description = request.Description;
        evt.PlaceId = request.PlaceId;
        evt.StartDateTime = request.StartDateTime;
        evt.EndDateTime = request.EndDateTime;
        evt.RoutePolyline = request.RoutePolyline;
        evt.MaxParticipants = request.MaxParticipants;
        evt.MinParticipants = request.MinParticipants;
        evt.MinPaceSecondsPerKm = request.MinPaceSecondsPerKm;
        evt.MaxPaceSecondsPerKm = request.MaxPaceSecondsPerKm;
        evt.Status = request.Status;
        evt.Visibility = request.Visibility;
        evt.Location = request.Location;
        evt.DistanceKm = request.DistanceKm;

        await _context.SaveChangesAsync();
        return Ok(ToDto(evt));
    }

    [Authorize]
    [HttpPost("")]
    public async Task<ActionResult<EventDto>> PostEvent(UpsertEventDto request)
    {
        var userProfile = await _currentUser.GetUserProfileAsync(User);
        if (userProfile is null) return Unauthorized();

        var evt = new Event{
            Title = request.Title,
            Description = request.Description,
            PlaceId = request.PlaceId,
            CreatedByUserProfileId = userProfile.Id,
            StartDateTime = request.StartDateTime,
            EndDateTime = request.EndDateTime,
            RoutePolyline = request.RoutePolyline,
            MaxParticipants = request.MaxParticipants,
            MinParticipants = request.MinParticipants,
            MinPaceSecondsPerKm = request.MinPaceSecondsPerKm,
            MaxPaceSecondsPerKm = request.MaxPaceSecondsPerKm,
            Status = request.Status,
            Visibility = request.Visibility,
            Location = request.Location,
            DistanceKm = request.DistanceKm
        };
        _context.Events.Add(evt);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, ToDto(evt));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(long id)
    {
        var userProfile = await _currentUser.GetUserProfileAsync(User);
        if (userProfile is null) return Unauthorized(); 

        var evt = await _context.Events.FindAsync(id);
        if (evt is null) return NotFound();

        if (evt.CreatedByUserProfileId != userProfile.Id) return Forbid();

        _context.Events.Remove(evt);
        await _context.SaveChangesAsync();
        return Ok();   
    }


    private static EventDto ToDto(Event Event)
    {
        return new EventDto(
            Event.Id,
            Event.Title,
            Event.Description,
            Event.PlaceId,
            Event.CreatedByUserProfileId,
            Event.StartDateTime,
            Event.EndDateTime,
            Event.RoutePolyline,
            Event.MaxParticipants,
            Event.MinParticipants,
            Event.MinPaceSecondsPerKm,
            Event.MaxPaceSecondsPerKm,
            Event.Status,
            Event.Visibility,
            Event.Location,
            Event.DistanceKm
        );
    }

    public sealed record EventDto(
        long Id,
        string Title,
        string? Description,
        long PlaceId,
        long CreatedByUserProfileId,
        DateTime StartDateTime,
        DateTime EndDateTime,
        string? RoutePolyline,
        int MaxParticipants,
        int MinParticipants,
        int MinPaceSecondsPerKm,
        int MaxPaceSecondsPerKm,
        string Status,
        string Visibility,
        string? Location,
        decimal? DistanceKm
    );

    public sealed record UpsertEventDto(
        string Title,
        string? Description,
        long PlaceId,
        DateTime StartDateTime,
        DateTime EndDateTime,
        string? RoutePolyline,
        int MaxParticipants,
        int MinParticipants,
        int MinPaceSecondsPerKm,
        int MaxPaceSecondsPerKm,
        string Status,
        string Visibility,
        string? Location,
        decimal? DistanceKm
    );

   

}