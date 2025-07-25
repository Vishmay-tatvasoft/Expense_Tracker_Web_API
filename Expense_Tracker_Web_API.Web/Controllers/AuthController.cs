using Expense_Tracker_Web_API.Services.Helpers;
using Expense_Tracker_Web_API.Services.Interfaces;
using Expense_Tracker_Web_API.Services.ViewModels;
using Expense_Tracker_Web_API.Web.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace Expense_Tracker_Web_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IJwtTokenService jwtTokenService, IConfiguration config) : ControllerBase
{
    #region Configuration Settings
    private readonly IAuthService _authService = authService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IConfiguration _config = config;
    #endregion

    #region Register User
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] string encryptedPayload)
    {
        SignUpVM? signUpVM = PayloadHelper.DecryptAndDeserialize<SignUpVM>(encryptedPayload);
        if (signUpVM == null) return BadRequest("Invalid encrypted payload");
        ApiResponseVM<UserVM> apiResponseVM = await _authService.RegisterUserAsync(signUpVM);
        return apiResponseVM.StatusCode switch
        {
            ApiStatusCode.Created => CreatedAtAction(nameof(RegisterUser), apiResponseVM.Data),
            _ => StatusCode((int)apiResponseVM.StatusCode, apiResponseVM)
        };
    }
    #endregion

    #region Login User
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginVM loginVM)
    {
        ApiResponseVM<UserVM> apiResponseVM = await _authService.LoginUserAsync(loginVM);

        if (apiResponseVM.StatusCode == ApiStatusCode.Success)
        {
            TokenResponseVM? tokenResponse = apiResponseVM.Data!.LoginData;
            DateTime expirationTime = tokenResponse!.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(7);
            SetCookie("ExpenseTrackerAccessToken", tokenResponse.AccessToken, expirationTime);
            SetCookie("ExpenseTrackerRefreshToken", tokenResponse.RefreshToken, expirationTime);
            return Ok(apiResponseVM);
        };
        return StatusCode((int)apiResponseVM.StatusCode, apiResponseVM);
    }
    #endregion

    #region Refresh Token
    [HttpGet("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["ExpenseTrackerRefreshToken"];
        RefreshTokenVM refreshTokenVM = new()
        {
            RefreshToken = refreshToken!,
            RememberMe = Convert.ToBoolean(_jwtTokenService.GetClaimValue(refreshToken!, "RememberMe"))
        };
        ApiResponseVM<TokenResponseVM> apiResponseVM = await _authService.RefreshTokenAsync(refreshTokenVM);
        if(apiResponseVM.StatusCode == ApiStatusCode.Success)
        {
            TokenResponseVM? tokenResponse = apiResponseVM.Data;
            DateTime expirationTime = tokenResponse!.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddDays(7);
            SetCookie("ExpenseTrackerAccessToken", tokenResponse.AccessToken, expirationTime);
            SetCookie("ExpenseTrackerRefreshToken", tokenResponse.RefreshToken, expirationTime);
            return Ok(apiResponseVM);
        }
        return StatusCode((int)apiResponseVM.StatusCode, apiResponseVM);
    }
    #endregion

    // #region Forgot Password
    // [HttpPost("forgotpassword")]
    // public async Task<IActionResult> ForgotPassword(string encryptedPayload)
    // {
    //     ApiResponseVM<object> apiResponseVM = await _authService.ForgotPasswordAsync()
    // }
    // #endregion

    #region Set Cookie
    private void SetCookie(string name, string value, DateTime expiryTime)
    {
        Response.Cookies.Append(name, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = expiryTime
        });
    }
    #endregion

    #region Remove Cookie
    private void RemoveCookie(string name)
    {
        Response.Cookies.Delete(name, new CookieOptions
        {
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
    #endregion

}
