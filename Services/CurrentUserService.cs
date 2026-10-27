using System.Security.Claims;
using RunnerMeets.Models;
using Microsoft.EntityFrameworkCore;

namespace RunnerMeets.Services;
public class CurrentUserService : ICurrentUserService
{
    private readonly ApplicationDbContext _context;

    public CurrentUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetUserProfileAsync(ClaimsPrincipal user)
    {
        var identityUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (identityUserId is null) return null;

        return await _context.UserProfiles
            .FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
    }
}