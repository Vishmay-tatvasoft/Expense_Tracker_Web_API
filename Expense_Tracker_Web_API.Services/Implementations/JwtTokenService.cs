using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expense_Tracker_Web_API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Expense_Tracker_Web_API.Services.Implementations;

public class JwtTokenService(IConfiguration configuration) : IJwtTokenService
{
    #region Configuration Settings
    private readonly string _key = configuration["Jwt:Key"]!;
    private readonly string _issuer = configuration["Jwt:Issuer"]!;
    private readonly string _audience = configuration["Jwt:Audience"]!;
    private readonly string _encryptkey = configuration["Jwt:EncryptKey"]!;
    #endregion

    #region Generate JWT Token
    public string GenerateJwtToken(string userName, string email, string userID)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_key);
        byte[] encryptKey = Encoding.UTF8.GetBytes(_encryptkey);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                        new Claim("UserName", userName),
                        new Claim("Email", email),
                        new Claim("UserID",userID),
                        new Claim(ClaimTypes.NameIdentifier,userID.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                    }),

            Expires = DateTime.UtcNow.AddMinutes(15),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            EncryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptKey),
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes128CbcHmacSha256
            )
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    #endregion

    #region Generate Referesh Toke
    public string GenerateRefreshTokenJwt(string userName, string email, string userID, bool rememberMe)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.UTF8.GetBytes(_key);
        byte[] encryptKey = Encoding.UTF8.GetBytes(_encryptkey);
        DateTime expiryTime = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(2);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                        new Claim("UserName", userName),
                        new Claim("Email", email),
                        new Claim("UserID",userID),
                        new Claim("RememberMe",rememberMe.ToString()),
                        new Claim(ClaimTypes.NameIdentifier,userID.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                }),

            Expires = expiryTime,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            EncryptingCredentials = new EncryptingCredentials(
                new SymmetricSecurityKey(encryptKey),
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes128CbcHmacSha256
            )
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    #endregion

    #region Validate JWT Token
    public (bool? isValid, bool? isExpired, ClaimsPrincipal principal) ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return (false, null, null)!; // missing
        try
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_key);
            byte[] encryptKey = Encoding.UTF8.GetBytes(_encryptkey);

            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                TokenDecryptionKey = new SymmetricSecurityKey(encryptKey),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return (true, false, principal); // valid and not expired
        }
        catch (SecurityTokenExpiredException)
        {
            return (false, true, null)!; // not valid and expired
        }
        catch
        {
            return (false, null, null)!; // malformed token
        }

    }
    #endregion

    #region Is Refresh Token Valid
    public bool IsRefreshTokenValid(string token)
    {
        var (isValid, isExpired, _) = ValidateToken(token);
        return (isValid ?? false) && !(isExpired ?? true);
    }
    #endregion

    #region Retrieves a specified claim from a JWT token.
    public ClaimsPrincipal GetClaimsFromToken(string token)
    {
        ClaimsPrincipal principal = ValidateToken(token).principal;
        return principal;
    }
    #endregion

    #region Retrieves a specific claim value from a JWT token.
    public string GetClaimValue(string token, string claimType)
    {
        ClaimsPrincipal claimsPrincipal = GetClaimsFromToken(token);

        string? value = claimsPrincipal?.FindFirst(claimType)?.Value;
        return value ?? string.Empty;
    }
    #endregion

}
