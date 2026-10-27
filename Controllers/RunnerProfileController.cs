using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMeets.Models;
using System.Security.Claims;

namespace RunnerMeets.Controllers;

[Route("api/runner-profile")]
[ApiController]
public class RunnerProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public RunnerProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<ActionResult<RunnerProfileDto>> UpsertMyRunnerProfile(UpsertRunnerProfileDto request)
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (identityUserId is null) return Unauthorized();

        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);

        if (userProfile is null) return NotFound("Create your user profile first.");

        var runnerProfile = await _context.RunnerProfiles
            .FirstOrDefaultAsync(x => x.UserProfileId == userProfile.Id);

        if (runnerProfile is null)
        {
            runnerProfile = new RunnerProfile
            {
                UserProfileId = userProfile.Id
            };

            _context.RunnerProfiles.Add(runnerProfile);
        }
        runnerProfile.Bio = request.Bio;
        runnerProfile.ExperienceLevel = request.ExperienceLevel;
        runnerProfile.MinPaceSecondsPerKm = request.MinPaceSecondsPerKm;
        runnerProfile.MaxPaceSecondsPerKm = request.MaxPaceSecondsPerKm;
        runnerProfile.TrainingFor = request.TrainingFor;
        runnerProfile.PreferredDistanceKm = request.PreferredDistanceKm;
        runnerProfile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ToDto(runnerProfile));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<RunnerProfileDto>> GetMyRunnerProfile()
    {
        var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (identityUserId is null) return Unauthorized();

        var userProfile = await _context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);

        if (userProfile is null) return NotFound("Create your user profile first.");

        var runnerProfile = await _context.RunnerProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserProfileId == userProfile.Id);

        if (runnerProfile is null) return NotFound("Runner profile not found.");

        return Ok(ToDto(runnerProfile));
    }
        

    private static RunnerProfileDto ToDto(RunnerProfile runnerProfile)
    {
        return new RunnerProfileDto(
            runnerProfile.Id,
            runnerProfile.UserProfileId,
            runnerProfile.Bio,
            runnerProfile.ExperienceLevel,
            runnerProfile.MinPaceSecondsPerKm,
            runnerProfile.MaxPaceSecondsPerKm,
            runnerProfile.TrainingFor,
            runnerProfile.PreferredDistanceKm,
            runnerProfile.CreatedAt,
            runnerProfile.UpdatedAt
        );
    }

    public sealed record RunnerProfileDto(
        long Id,
        long UserProfileId,
        string? Bio,
        string? ExperienceLevel,
        int? MinPaceSecondsPerKm,
        int? MaxPaceSecondsPerKm,
        string? TrainingFor,
        decimal? PreferredDistanceKm,
        DateTime CreatedAt,
        DateTime UpdatedAt
    );

    public sealed record UpsertRunnerProfileDto(
        string? Bio,
        string? ExperienceLevel,
        int? MinPaceSecondsPerKm,
        int? MaxPaceSecondsPerKm,
        string? TrainingFor,
        decimal? PreferredDistanceKm
    );

}
