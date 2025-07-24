using Expense_Tracker_Web_API.Services.Helpers;
using Expense_Tracker_Web_API.Services.Interfaces;
using Expense_Tracker_Web_API.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker_Web_API.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    #region Configuration Settings
    private readonly IAuthService _authService = authService;
    #endregion

    #region Register User
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] SignUpVM signUpVM)
    {
        ApiResponseVM<UserVM> apiResponseVM = await _authService.RegisterUserAsync(signUpVM);
        return apiResponseVM.StatusCode switch
        {
            ApiStatusCode.Created => CreatedAtAction(nameof(RegisterUser), apiResponseVM.Data),
            _ => StatusCode((int)apiResponseVM.StatusCode, apiResponseVM.Message)
        };
    }
    #endregion

    #region Login User
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginVM loginVM)
    {
        ApiResponseVM<UserVM> apiResponseVM = await _authService.LoginUserAsync(loginVM);
        return apiResponseVM.StatusCode switch
        {
            ApiStatusCode.Success => Ok(apiResponseVM.Data),
            _ => StatusCode((int)apiResponseVM.StatusCode, apiResponseVM)
        };
    }
    #endregion

}
