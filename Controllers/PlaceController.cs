using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMeets.Models;

namespace RunnerMeets.Controllers;

[Route("api/places")]
[ApiController]
public class PlaceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public PlaceController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet("")]
    public async Task<ActionResult<List<PlaceDto>>> GetPlaces()
    {
        var places = await _context.Places
            .AsNoTracking()
            .ToListAsync();

        var placeDtos = places.Select(p => ToDto(p)).ToList();
        return Ok(placeDtos);
    }


    [HttpGet("{id}")]  
    public async Task<ActionResult<PlaceDto>> GetPlace(long id)
    {
        var place = await _context.Places.FindAsync(id);
        if (place is null) return NotFound();
        
        return Ok(ToDto(place));
    }
    
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<PlaceDto>> UpdatePlace(long id, UpsertPlaceDto request)
    {
        var place = await _context.Places.FindAsync(id);
        if (place is null) return NotFound();

        place.GooglePlaceId = request.GooglePlaceId;
        place.Name = request.Name;
        place.FormattedAddress = request.FormattedAddress;
        place.PlaceType = request.PlaceType;
        place.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Ok(ToDto(place));
    }

    [Authorize]
    [HttpPost("")]
    public async Task<ActionResult<PlaceDto>> PostPlace(UpsertPlaceDto request)
    {
        var place = new Place
        {
            GooglePlaceId = request.GooglePlaceId,
            Name = request.Name,
            FormattedAddress = request.FormattedAddress,
            PlaceType = request.PlaceType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Places.Add(place);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPlace), new { id = place.Id }, ToDto(place));
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlace(long id)
    {
        //TODO: probably want only admins to be able to delete places, but for now, just allow any authenticated user to delete a place
        var place = await _context.Places.FindAsync(id);
        if (place is null) return NotFound();

        _context.Places.Remove(place);
        await _context.SaveChangesAsync();
        return NoContent();
    }


    public static PlaceDto ToDto(Place place) => new PlaceDto(
        place.Id,
        place.GooglePlaceId,
        place.Name,
        place.FormattedAddress,
        place.PlaceType,
        place.CreatedAt,
        place.UpdatedAt
    );

    public sealed record PlaceDto(
        long Id,
        string? GooglePlaceId,
        string? Name,
        string? FormattedAddress,
        string? PlaceType,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    public sealed record UpsertPlaceDto(
        string? GooglePlaceId,
        string? Name,
        string? FormattedAddress,
        string? PlaceType
    );


}
