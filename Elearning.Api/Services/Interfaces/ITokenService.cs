using Elearning.Api.Models;

namespace Elearning.Api.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}
