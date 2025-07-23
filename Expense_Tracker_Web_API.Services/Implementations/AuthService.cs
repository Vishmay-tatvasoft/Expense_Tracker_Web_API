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
        if (_userRepository.CheckForExistingUserAsync(signUpVM.Email) != null)
        {
            return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.Conflict, MessageHelper.UserAlreadyExists);
        }
        #endregion
        User newUser = new()
        {
            Name = signUpVM.Name,
            Email = signUpVM.Email,
            Passwordhash = PasswordHelper.HashPassword(signUpVM.Password)
        };
        bool isRegistered = await _userGR.AddRecordAsync(newUser);
        if (!isRegistered) return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.ServerError, MessageHelper.RegistrationFailed);
        UserVM userVM = new()
        {
            UserID = newUser.UserId,
            Name = newUser.Name,
            Email = newUser.Email
        };

        return ApiResponseFactory.Success(ApiStatusCode.Created, MessageHelper.UserRegistered, userVM);
    }
    #endregion

    #region Login User Async
    public async Task<ApiResponseVM<UserVM>> LoginUserAsync(LoginVM loginVM)
    {
        User? existingUser = await _userRepository.CheckForExistingUserAsync(loginVM.EmailAddress);
        if (existingUser != null && PasswordHelper.VerifyPassword(loginVM.Password, existingUser.Passwordhash))
        {
            return ApiResponseFactory.Success(ApiStatusCode.Success, MessageHelper.UserLoggedIn, new UserVM
            {
                UserID = existingUser.UserId,
                Name = existingUser.Name,
                Email = existingUser.Email
            });
        }
        return ApiResponseFactory.Fail<UserVM>(ApiStatusCode.Unauthorized, MessageHelper.InvalidCredentials);
    }
    #endregion

}
