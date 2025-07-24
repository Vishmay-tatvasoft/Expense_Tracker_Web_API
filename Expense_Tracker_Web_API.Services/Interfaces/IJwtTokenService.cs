using System.Security.Claims;

namespace Expense_Tracker_Web_API.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateJwtToken(string userName, string email, string userID);
    string GenerateRefreshTokenJwt(string userName, string email, string userID, bool rememberMe);
    bool IsRefreshTokenValid(string token);
    (bool? isValid, bool? isExpired, ClaimsPrincipal principal) ValidateToken(string token);
    ClaimsPrincipal GetClaimsFromToken(string token);
    string GetClaimValue(string token, string claimType);
}
