using Expense_Tracker_Web_API.Repositories.Interfaces;
using Expense_Tracker_Web_API.Repositories.Models;
using Expense_Tracker_Web_API.Services.Helpers;
using Expense_Tracker_Web_API.Services.Interfaces;
using Expense_Tracker_Web_API.Services.ViewModels;

namespace Expense_Tracker_Web_API.Services.Implementations;

public class AuthService(IGenericRepository<User> userGR, IUserRepository userRepository) : IAuthService
{
    #region Configuration Settings
    private readonly IGenericRepository<User> _userGR = userGR;
    private readonly IUserRepository _userRepository = userRepository;
    #endregion


    #region Register User Async
    public async Task<ApiResponseVM<UserVM>> RegisterUserAsync(SignUpVM signUpVM)
    {
        #region Check If User Already Exists
        if(await _userRepository.CheckForExistingUserAsync(signUpVM.Email))
        {
            return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.Conflict, "User with this email already exists");
        }
        #endregion
        User newUser = new()
        {
            Name = signUpVM.Name,
            Email = signUpVM.Email,
            Passwordhash = PasswordHelper.HashPassword(signUpVM.Password)
        };
        bool isRegistered = await _userGR.AddRecordAsync(newUser);
        if (!isRegistered) return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.ServerError, "User registration failed");
        UserVM userVM = new()
        {
            UserID = newUser.UserId,
            Name = newUser.Name,
            Email = newUser.Email
        };

        return ApiResponseFactory.Success(ApiStatusCode.Created, "User registered successfully", userVM);
    }
    #endregion

}
