using System.Security.Claims;
using RunnerMeets.Models;

namespace RunnerMeets.Services;

public interface ICurrentUserService
{
    Task<UserProfile?> GetUserProfileAsync(ClaimsPrincipal user);
}