using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMeets.Models;
using System.Security.Claims;

namespace RunnerMeets.Controllers;

[Route("api/user-profile")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public UserProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetMyProfile()
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (identityUserId is null) return Unauthorized();

        var profile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(userProfile => userProfile.IdentityUserId == identityUserId);

        if (profile is null) return NotFound("User profile not found");

        return Ok(ToDto(profile));
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<UserProfileDto>> UpsertMyProfile(UpsertUserProfileDto request)
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (identityUserId is null) return Unauthorized();

        var identityUser = await _userManager.FindByIdAsync(identityUserId);
        if (identityUser is null) return Unauthorized();

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(userProfile => userProfile.IdentityUserId == identityUserId);

        if (profile is null)
        {
            profile = new UserProfile
            {
                IdentityUserId = identityUserId,
            };

            _context.UserProfiles.Add(profile);
        }

        profile.DisplayName = request.DisplayName;
        profile.FirstName = request.FirstName;
        profile.LastName = request.LastName;
        profile.DateOfBirth = request.DateOfBirth;
        profile.Postcode = request.Postcode;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ToDto(profile));
    }

    private static UserProfileDto ToDto(UserProfile profile)
    {
        return new UserProfileDto(
            profile.Id,
            profile.DisplayName,
            profile.FirstName,
            profile.LastName,
            profile.DateOfBirth,
            profile.Postcode);
    }

    public sealed record UserProfileDto(
        long Id,
        string? DisplayName,
        string? FirstName,
        string? LastName,
        DateTime? DateOfBirth,
        string? Postcode);

    public sealed record UpsertUserProfileDto(
        string? DisplayName,
        string? FirstName,
        string? LastName,
        DateTime? DateOfBirth,
        string? Postcode);
}
